using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate.Events;
using ASM.Application.Domain.ReturningRequestAggregate;
using MediatR;

namespace ASM.Application.Features.Assignments.EventHandlers;

public sealed class AssignmentRequestedForReturningHandler(IRepository<ReturningRequest> repository)
    : INotificationHandler<AssignmentRequestedForReturningEvent>
{
    public async Task Handle(AssignmentRequestedForReturningEvent notification, CancellationToken cancellationToken)
    {
        ReturningRequest returningRequest = new(notification.AssetAssignId);
        await repository.AddAsync(returningRequest, cancellationToken);
    }
}
