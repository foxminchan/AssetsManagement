using ASM.Application.Domain.IdentityAggregate.Enums;

namespace ASM.Application.Features.Staffs;

public sealed record StaffDto(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? FullName,
    string? StaffCode,
    string? UserName,
    DateOnly JoinedDate,
    DateOnly Dob,
    RoleType RoleType,
    Gender Gender,
    Location Location);
