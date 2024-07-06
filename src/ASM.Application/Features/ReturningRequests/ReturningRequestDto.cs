using ASM.Application.Domain.ReturningRequestAggregate.Enums;

namespace ASM.Application.Features.ReturningRequests;

public sealed record ReturningRequestDto(
    int No,
    Guid Id,
    string? AssetCode,
    string? AssetName,
    string? RequestedBy,
    DateOnly? AssignedDate,
    string? AcceptedBy,
    DateOnly? ReturnedDate,
    State State);
