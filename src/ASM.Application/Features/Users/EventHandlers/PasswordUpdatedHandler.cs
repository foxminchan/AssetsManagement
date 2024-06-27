using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.IdentityAggregate.Events;
using ASM.Application.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ASM.Application.Features.Users.EventHandlers;

public sealed class PasswordUpdatedHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    : INotificationHandler<PasswordUpdatedEvent>
{
    public async Task Handle(PasswordUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var user = notification.User;

        var claims = await userManager.GetClaimsAsync(user);
        var statusClaim = claims.First(c => c.Type == "Status");

        if (statusClaim.Value == nameof(AccountStatus.FirstTime))
        {
            await userManager.RemoveClaimAsync(user, statusClaim);
            await userManager.AddClaimAsync(user, new("Status", nameof(AccountStatus.Active)));
        }

        context.Entry(user).State = EntityState.Detached;
    }
}
