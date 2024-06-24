using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.IdentityAggregate.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ASM.Application.Features.Staffs.EventHandlers;

public sealed class DeleteStaffHandler(UserManager<ApplicationUser> userManager)
    : INotificationHandler<StaffDeletedEvent>
{
    public async Task Handle(StaffDeletedEvent notification, CancellationToken cancellationToken)
    {
        notification.User.LockoutEnd = DateTimeOffset.MaxValue;
        await userManager.AddClaimAsync(notification.User, new("Status", nameof(AccountStatus.Deactivated)));
    }
}
