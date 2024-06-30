using System.Security.Claims;
using ASM.Application.Common.Constants;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.Shared;
using ASM.Application.Infrastructure.Persistence;
using ASM.IntegrationTest.Fixtures;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ASM.IntegrationTest.Extensions;

public static class ApplicationExtension
{
    public static async Task EnsureCreatedAsync<T>(
        this ApplicationFactory<T> factory,
        CancellationToken cancellationToken = default)
        where T : class
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }

    public static async Task EnsureCreatedAndPopulateDataAsync<TProgram, TEntity>(
        this ApplicationFactory<TProgram> factory,
        IReadOnlyCollection<TEntity> entities,
        CancellationToken cancellationToken = default)
        where TProgram : class
        where TEntity : class
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public static async Task EnsureCreatedAndPopulateIdentityUserClaimsAsync<TProgram, TUser>(
        this ApplicationFactory<TProgram> factory,
        TUser user)
        where TProgram : class
        where TUser : IdentityUser
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
        var result = await userManager.CreateAsync(user, "P@ssw0rd");

        if (!result.Succeeded)
            throw new InvalidOperationException(
                result.Errors.Select(e => e.Description).Aggregate((a, b) => $"{a}{Environment.NewLine}{b}"));

        await userManager.AddClaimsAsync(user,
        [
            new(nameof(AuthRole), AuthRole.Admin),
            new("Status", nameof(AccountStatus.Active)),
            new(nameof(ApplicationUser.UserName), user.UserName!),
            new(nameof(Location), nameof(Location.HoChiMinh)),
            new(ClaimTypes.Role, AuthRole.Admin)
        ]);
    }
}
