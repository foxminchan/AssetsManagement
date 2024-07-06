using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;

namespace ASM.Application.Domain.ReturningRequestAggregate.Events;

public sealed class ReturningRequestCancelledEvent(Guid assignmentId) : EventBase
{
    public Guid AssignmentId { get; set; } = Guard.Against.Null(assignmentId);
}
