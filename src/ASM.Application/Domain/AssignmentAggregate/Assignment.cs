using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using ASM.Application.Domain.IdentityAggregate;

namespace ASM.Application.Domain.AssignmentAggregate;

public sealed class Assignment : TrackableEntityBase, IAggregateRoot
{
    public Assignment()
    {
        // EF Mapping
    }

    public Assignment(State state, DateOnly assignedDate, string? note, Guid assetId, Guid staffId)
    {
        State = Guard.Against.EnumOutOfRange(state);
        AssignedDate = Guard.Against.NullOrOutOfRange(assignedDate, nameof(assignedDate),
            DateOnly.FromDateTime(DateTime.Now), DateOnly.MaxValue);
        Note = note;
        AssetId = Guard.Against.Null(assetId);
        StaffId = Guard.Against.Null(staffId);
    }

    public State State { get; set; }
    public DateOnly AssignedDate { get; set; }
    public string? Note { get; set; }
    public Guid? AssetId { get; set; }
    public Asset? Asset { get; set; }
    public Guid StaffId { get; set; }
    public Staff? Staff { get; set; }

    public void UpdateState(State state) => State = Guard.Against.EnumOutOfRange(state);

    [NotMapped] public string? AssignedBy { get; set; }
    [NotMapped] public string? AssignedTo { get; set; }
}
