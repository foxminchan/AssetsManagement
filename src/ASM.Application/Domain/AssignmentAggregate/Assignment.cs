﻿using System.ComponentModel.DataAnnotations.Schema;
using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using ASM.Application.Domain.AssignmentAggregate.Events;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.ReturningRequestAggregate;

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
    public ICollection<ReturningRequest>? ReturningRequests = [];

    public void UpdateState(State state) => State = Guard.Against.EnumOutOfRange(state);

    [NotMapped] public string? AssignedBy { get; set; }
    [NotMapped] public string? AssignedTo { get; set; }

    public void Update(Guid userId, Guid assetId, DateOnly assignedDate, string note)
    {
        StaffId = Guard.Against.Null(userId);
        AssetId = Guard.Against.Null(assetId);
        AssignedDate = assignedDate;
        Note = note;
    }

    public void UpdateAssetState(Guid assetId)
    {
        var assetCreatedEvent = new AssignmentCreatedEvent(assetId);
        RegisterDomainEvent(assetCreatedEvent);
    }

    public void UpdateAssetState(Guid newAssetAssignId, Guid? oldAssetAssignId)
    {
        var assetUpdatedEvent = new AssignmentUpdatedEvent(newAssetAssignId, oldAssetAssignId);
        RegisterDomainEvent(assetUpdatedEvent);
    }

    public void RequestForReturning(Guid id)
    {
        var assignmentRequestedForReturningEvent = new AssignmentRequestedForReturningEvent(id);
        RegisterDomainEvent(assignmentRequestedForReturningEvent);
    }
}
