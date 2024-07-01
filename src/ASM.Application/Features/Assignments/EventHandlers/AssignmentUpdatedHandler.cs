using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.AssignmentAggregate.Events;
using MediatR;

namespace ASM.Application.Features.Assignments.EventHandlers;

public class AssignmentUpdatedHandler(IRepository<Asset> repository)
    : INotificationHandler<AssignmentUpdatedEvent>
{
    public async Task Handle(AssignmentUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var oldAssetTask = repository.GetByIdAsync(notification.OldAssetAssignId, cancellationToken);
        var newAssetTask = repository.GetByIdAsync(notification.NewAssetAssignId, cancellationToken);

        await Task.WhenAll(oldAssetTask, newAssetTask);

        var oldAsset = await oldAssetTask;
        var newAsset = await newAssetTask;

        if (newAsset is not null && oldAsset is not null)
        {
            oldAsset.Assignments = null;
            oldAsset.Category = null;

            newAsset.Category = null;
            oldAsset.Assignments = null;
            newAsset.Assignments = null;

            oldAsset.State = State.Available;
            newAsset.State = State.Assigned;

            await repository.UpdateAsync(oldAsset, cancellationToken);
            await repository.UpdateAsync(newAsset, cancellationToken);

        }
    }
}
