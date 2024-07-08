using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.AssignmentAggregate.Events;
using ASM.Application.Infrastructure.Persistence;
using MediatR;

namespace ASM.Application.Features.Assignments.EventHandlers;
public class AssignmentUpdatedHandler(IRepository<Asset> repository)
    : INotificationHandler<AssignmentUpdatedEvent>
{
    public async Task Handle(AssignmentUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var oldAsset = await repository.GetByIdAsync(notification.OldAssetAssignId, cancellationToken);
        var newAsset = await repository.GetByIdAsync(notification.NewAssetAssignId, cancellationToken);

        if (newAsset is not null && oldAsset is not null)
        {
            oldAsset.UpdateState(State.Available);
            newAsset.UpdateState(State.Assigned);

            await repository.SaveChangesAsync(cancellationToken);
        }
    }
}
