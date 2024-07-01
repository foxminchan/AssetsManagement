using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.AssignmentAggregate.Events;
using MediatR;

namespace ASM.Application.Features.Assignments.EventHandlers;

public class AssignmentCreatedHandler(IRepository<Asset> repository)
    : INotificationHandler<AssignmentCreatedEvent>
{
    public async Task Handle(AssignmentCreatedEvent notification, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByIdAsync(notification.AssetAssignId, cancellationToken);

        if (asset is not null)
        {
            asset.Assignments = null;
            asset.Category = null;
            asset.State = State.Assigned;
            await repository.UpdateAsync(asset, cancellationToken);
        }
    }
}
