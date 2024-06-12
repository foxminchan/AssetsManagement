using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;
using ASM.Application.Domain.IdentityAggregate.Enums;

namespace ASM.Application.Domain.IdentityAggregate.Events;

public sealed class CreatedStaffEvent(
    string firstName,
    string lastName,
    RoleType roleType,
    DateOnly dob,
    string location,
    Guid staffId) : EventBase
{
    public string FirstName { get; set; } = Guard.Against.NullOrEmpty(firstName);
    public string LastName { get; set; } = Guard.Against.NullOrEmpty(lastName);
    public RoleType RoleType { get; set; } = Guard.Against.Null(roleType);
    public DateOnly Dob { get; set; } = Guard.Against.Null(dob);
    public string Location { get; set; } = Guard.Against.Null(location);
    public Guid StaffId { get; set; } = Guard.Against.NullOrEmpty(staffId);
}
