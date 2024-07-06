using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using ASM.Application.Domain.ReturningRequestAggregate.Events;
using MediatR;

namespace ASM.Application.Features.ReturningRequests.EventHandlers;

public sealed class ReturningRequestCompletedHandler(IRepository<Assignment> repository)
    : INotificationHandler<ReturningRequestCompletedEvent>
{
    public async Task Handle(ReturningRequestCompletedEvent notification, CancellationToken cancellationToken)
    {
        var assignment = await repository.GetByIdAsync(notification.AssignmentId, cancellationToken);

        Guard.Against.NotFound(notification.AssignmentId, assignment);

        assignment.UpdateState(State.Returned);

        await repository.SaveChangesAsync(cancellationToken);
    }
}
