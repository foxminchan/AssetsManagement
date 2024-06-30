using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.Shared;

namespace ASM.Application.Features.Assets;

public sealed record AssetDto(
    Guid Id,
    string? Name,
    string? AssetCode,
    string? Specification,
    string? Category,
    DateOnly InstallDate,
    State State,
    Location Location,
    Guid CategoryId);
