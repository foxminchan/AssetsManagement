using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.ReturningRequestAggregate.Events;
using MediatR;
using AssetState = ASM.Application.Domain.AssetAggregate.Enums;
using AssignmentState = ASM.Application.Domain.AssignmentAggregate.Enums;

namespace ASM.Application.Features.ReturningRequests.EventHandlers;

public sealed class ReturningRequestCompletedHandler(IRepository<Assignment> repository, IRepository<Asset> assetRepository)
    : INotificationHandler<ReturningRequestCompletedEvent>
{
    public async Task Handle(ReturningRequestCompletedEvent notification, CancellationToken cancellationToken)
    {
        var assignment = await repository.GetByIdAsync(notification.AssignmentId, cancellationToken);
        Guard.Against.NotFound(notification.AssignmentId, assignment);
        var asset = await assetRepository.GetByIdAsync((Guid)assignment.AssetId!, cancellationToken);
        Guard.Against.NotFound((Guid)assignment.AssetId!, asset);

        asset.UpdateState(AssetState.State.Recycled);
        assignment.UpdateState(AssignmentState.State.Returned);

        await repository.SaveChangesAsync(cancellationToken);
    }
}
