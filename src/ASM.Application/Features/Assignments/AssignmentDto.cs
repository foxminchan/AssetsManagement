using ASM.Application.Domain.AssignmentAggregate.Enums;

namespace ASM.Application.Features.Assignments;

public sealed record AssignmentDto(
    int No,
    Guid Id,
    Guid? AssetId,
    Guid UserId,
    string? AssetCode,
    string? AssetName,
    string? Specification,
    string? Category,
    string? AssignedTo,
    string? AssignedBy,
    DateOnly AssignedDate,
    State State,
    string? Note);
