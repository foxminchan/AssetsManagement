using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;

namespace ASM.Application.Domain.AssignmentAggregate.Events;

public sealed class AssignmentUpdatedEvent(Guid? newAssetAssignId, Guid? oldAssetAssignId) : EventBase
{
    public Guid NewAssetAssignId { get; set; } = Guard.Against.Null(newAssetAssignId);
    public Guid OldAssetAssignId { get; set; } = Guard.Against.Null(oldAssetAssignId);
}
