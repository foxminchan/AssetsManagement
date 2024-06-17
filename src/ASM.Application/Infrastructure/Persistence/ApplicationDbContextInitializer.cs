using ASM.Application.Common.Constants;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Infrastructure.Persistence.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASM.Application.Infrastructure.Persistence;

public static class InitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

        await initializer.InitialiseAsync();

        await initializer.SeedAsync();
    }
}

public sealed class ApplicationDbContextInitializer(
    ILogger<ApplicationDbContextInitializer> logger,
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw new DatabaseInitializationException("An error occurred while migrating the database.", ex);
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw new DatabaseInitializationException("An error occurred while seeding the database.", ex);
        }
    }

    public async Task TrySeedAsync()
    {
        await SeedRole();

        var users = GetUsers();

        foreach (var (roleType, userList) in users)
        {
            foreach (var user in userList)
            {
                await SeedUser(user, roleType);
            }
        }
    }

    private async Task SeedRole()
    {
        var adminRole = new IdentityRole(AuthRole.Admin);

        if (await roleManager.Roles.AllAsync(r => r.Name != adminRole.Name))
        {
            await roleManager.CreateAsync(adminRole);
        }

        var userRole = new IdentityRole(AuthRole.User);

        if (await roleManager.Roles.AllAsync(r => r.Name != userRole.Name))
        {
            await roleManager.CreateAsync(userRole);
        }
    }

    private async Task SeedUser(Staff user, RoleType roleType)
    {
        if (await context.Staffs.AllAsync(x => x.FirstName != user.FirstName))
        {
            // Insert User Data
            var staffCode = Staff.GenerateStaffCode(await context.Staffs.ToListAsync());

            user.StaffCode = staffCode;

            await context.Staffs.AddAsync(user);
            await context.SaveChangesAsync();

            // Insert ApplicationUser Data
            var users = await userManager.Users.ToListAsync();

            var userName = ApplicationUser.GenerateUserName(user.FirstName!, user.LastName!, users);

            ApplicationUser applicationUser = new()
            {
                UserName = userName, 
                AccountStatus = AccountStatus.FirstTime,
                StaffId = user.Id
            };

            if (user.IsDeleted)
            {
                applicationUser.AccountStatus = AccountStatus.Deactivated;
                applicationUser.LockoutEnd = DateTimeOffset.MaxValue;
            }

            var password = ApplicationUser.GeneratePassword(userName, user.Dob);

            var result = await userManager.CreateAsync(applicationUser, password);

            if (!result.Succeeded)
                throw new ValidationException(result.Errors.First().Description);

            var role = roleType switch
            {
                RoleType.Admin => AuthRole.Admin,
                RoleType.Staff => AuthRole.User,
                _ => throw new ArgumentOutOfRangeException(nameof(roleType), roleType, null)
            };

            await userManager.AddToRoleAsync(applicationUser, role);

            await userManager.AddClaimsAsync(applicationUser,
            [
                new(nameof(AuthRole), role),
                new("Status", nameof(AccountStatus.FirstTime)),
                new(nameof(ApplicationUser.UserName), userName),
                new(nameof(Location), user.Location.ToString())
            ]);

            logger.LogInformation("User {FirstName} {LastName} created.", user.FirstName, user.LastName);
        }
        else
        {
            logger.LogInformation("User {FirstName} {LastName} already exists.", user.FirstName,
                user.LastName);
        }
    }

    private static Dictionary<RoleType, List<Staff>> GetUsers() =>
        new()
        {
            {
                RoleType.Admin, [
                    new()
                    {
                        FirstName = "Nhan",
                        LastName = "Nguyen Xuan",
                        Location = Location.HoChiMinh,
                        Dob = new(2001, 8, 2),
                        JoinedDate = new(2024, 6, 17),
                        Gender = Gender.Male
                    },

                    new()
                    {
                        FirstName = "Dien",
                        LastName = "Truong Kim",
                        Location = Location.DaNang,
                        Dob = new(2002, 9, 3),
                        JoinedDate = new(2024, 6, 17),
                        Gender = Gender.Male
                    },

                    new()
                    {
                        FirstName = "Minh",
                        LastName = "Nguyen Le",
                        Location = Location.HaNoi,
                        Dob = new(2002, 6, 17),
                        JoinedDate = new(2024, 6, 17),
                        Gender = Gender.Male
                    }
                ]
            },
            {
                RoleType.Staff, [
                    new()
                    {
                        FirstName = "Man",
                        LastName = "Vo Minh",
                        Location = Location.HoChiMinh,
                        Dob = new(2002, 3, 17),
                        JoinedDate = new(2024, 6, 17),
                        Gender = Gender.Male,
                        IsDeleted = true,
                        RoleType = RoleType.Staff
                    },
                    new()
                    {
                        FirstName = "Khoi",
                        LastName = "Tran Minh",
                        Location = Location.HoChiMinh,
                        Dob = new(2003, 7, 17),
                        JoinedDate = new(2024, 6, 17),
                        Gender = Gender.Male,
                        RoleType = RoleType.Staff
                    }
                ]
            }
        };
}
