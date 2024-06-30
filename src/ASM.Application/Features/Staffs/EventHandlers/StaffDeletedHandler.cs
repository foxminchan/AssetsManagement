using Ardalis.GuardClauses;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.IdentityAggregate.Events;
using ASM.Application.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASM.Application.Features.Staffs.EventHandlers;

public sealed class StaffDeletedHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    : INotificationHandler<StaffDeletedEvent>
{
    public async Task Handle(StaffDeletedEvent notification, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(notification.UserId);
        Guard.Against.NotFound(notification.UserId, user);

        user.LockoutEnd = DateTimeOffset.MaxValue;
        var claims = await userManager.GetClaimsAsync(user);
        var statusClaim = claims.First(c => c.Type == "Status");

        await userManager.RemoveClaimAsync(user, statusClaim);
        await userManager.AddClaimAsync(user, new("Status", nameof(AccountStatus.Deactivated)));

        context.Entry(user).State = EntityState.Detached;
    }
}
