using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using ASM.Application.Domain.ReturningRequestAggregate.Events;
using MediatR;

namespace ASM.Application.Features.ReturningRequests.EventHandlers;

public sealed class ReturningRequestCancelledHandler(IRepository<Assignment> repository)
    : INotificationHandler<ReturningRequestCancelledEvent>
{
    public async Task Handle(ReturningRequestCancelledEvent notification, CancellationToken cancellationToken)
    {
        var assignment = await repository.GetByIdAsync(notification.AssignmentId, cancellationToken);

        Guard.Against.NotFound(notification.AssignmentId, assignment);

        assignment.UpdateState(State.Accepted);

        await repository.SaveChangesAsync(cancellationToken);
    }
}
