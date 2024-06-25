using System.ComponentModel.DataAnnotations;
using ASM.Application.Common.Constants;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.IdentityAggregate.Events;
using ASM.Application.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASM.Application.Features.Staffs.EventHandlers;

public sealed class StaffCreatedHandler(UserManager<ApplicationUser> userManager)
    : INotificationHandler<StaffCreatedEvent>
{
    public async Task Handle(StaffCreatedEvent notification, CancellationToken cancellationToken)
    {
        var appUsers = await userManager.Users.ToListAsync(cancellationToken);
        var userName = ApplicationUser.GenerateUserName(notification.FirstName, notification.LastName, appUsers);

        var user = new ApplicationUser
        {
            UserName = userName,
            StaffId = notification.StaffId,
            AccountStatus = AccountStatus.FirstTime
        };

        var password = ApplicationUser.GeneratePassword(userName, notification.Dob);

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.First().Description);

        var role = notification.RoleType switch
        {
            RoleType.Admin => AuthRole.Admin,
            RoleType.Staff => AuthRole.User,
            _ => AuthRole.User
        };

        await userManager.AddToRoleAsync(user, role);

        await userManager.AddClaimsAsync(user,
        [
            new(nameof(AuthRole), role),
            new(nameof(Location), notification.Location),
            new("Status", nameof(AccountStatus.FirstTime)),
            new(nameof(ApplicationUser.UserName), userName)
        ]);
    }
}
