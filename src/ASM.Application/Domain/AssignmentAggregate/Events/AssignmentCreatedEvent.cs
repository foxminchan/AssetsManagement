using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;

namespace ASM.Application.Domain.AssignmentAggregate.Events;

public sealed class AssignmentCreatedEvent(Guid assetAssignId) : EventBase
{
    public Guid AssetAssignId { get; set; } = Guard.Against.NullOrEmpty(assetAssignId);
}
