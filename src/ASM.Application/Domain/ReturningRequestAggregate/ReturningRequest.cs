using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.ReturningRequestAggregate.Enums;

namespace ASM.Application.Domain.ReturningRequestAggregate;

public sealed class ReturningRequest : TrackableEntityBase, IAggregateRoot
{
    private ReturningRequest()
    {
        // EF Mapping
    }

    public ReturningRequest(Guid assignmentId)
    {
        State = State.WaitingForReturning;
        AssignmentId = Guard.Against.Null(assignmentId);
    }

    public State State { get; private set; }
    public DateOnly? ReturnedDate { get; private set; }
    public Guid AssignmentId { get; private set; }
    public Staff? Staff { get; private set; } = default!;
    public Guid? AcceptBy { get; private set; }
    public Assignment? Assignment { get; private set; } = default!;

    public void MarkComplete(Guid acceptBy)
    {
        State = State.Completed;
        ReturnedDate = DateOnly.FromDateTime(DateTime.Now);
        AcceptBy = Guard.Against.Null(acceptBy);
    }
}
