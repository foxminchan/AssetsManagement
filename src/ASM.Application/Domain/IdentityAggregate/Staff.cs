using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.IdentityAggregate.Events;
using ASM.Application.Domain.Shared;

namespace ASM.Application.Domain.IdentityAggregate;

public sealed class Staff : EntityBase, ISoftDelete, IAggregateRoot
{
    public Staff()
    {
        // EF Mapping
    }

    public Staff(string? firstName, string? lastName, DateOnly dob, DateOnly joinedDate, Gender gender,
        string? staffCode, RoleType roleType, Location location)
    {
        FirstName = Guard.Against.NullOrEmpty(firstName);
        LastName = Guard.Against.NullOrEmpty(lastName);
        StaffCode = Guard.Against.NullOrEmpty(staffCode);
        Dob = dob;
        JoinedDate = joinedDate;
        Gender = Guard.Against.EnumOutOfRange(gender);
        RoleType = Guard.Against.EnumOutOfRange(roleType);
        Location = Guard.Against.EnumOutOfRange(location);
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? StaffCode { get; set; }
    public DateOnly Dob { get; set; }
    public DateOnly JoinedDate { get; set; }
    public Gender Gender { get; set; }
    public RoleType RoleType { get; set; }
    public Location Location { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<ApplicationUser>? Users { get; set; } = [];
    public ICollection<Assignment>? Assignments { get; set; } = [];

    public string FullName => $"{FirstName} {LastName}";
    public string? UserName => Users?.First().UserName;

    public static string GenerateStaffCode(List<Staff> staffs)
    {
        var staffCode = "SD0001";
        var count = 1;

        while (staffs.Exists(x => x.StaffCode == staffCode))
        {
            staffCode = $"SD{count:D4}";
            count++;
        }

        return staffCode;
    }

    public void Update(DateOnly dob, DateOnly joinedDate, Gender gender, RoleType roleType)
    {
        Dob = dob;
        JoinedDate = joinedDate;
        Gender = Guard.Against.EnumOutOfRange(gender);
        RoleType = Guard.Against.EnumOutOfRange(roleType);
    }

    public void CreateStaffAccount(string firstName, string lastName, RoleType roleType, DateOnly dob,
        string location, Guid staffId)
    {
        var createdStaffEvent = new StaffCreatedEvent(firstName, lastName, roleType, dob, location, staffId);
        RegisterDomainEvent(createdStaffEvent);
    }

    public void Delete() => IsDeleted = true;

    public void UpdateDeactivatedClaim(string userId)
    {
        StaffDeletedEvent staffDeletedEvent = new(userId);
        RegisterDomainEvent(staffDeletedEvent);
    }

    public void UpdateActiveClaim(ApplicationUser user)
    {
        PasswordUpdatedEvent passwordUpdatedEvent = new(user);
        RegisterDomainEvent(passwordUpdatedEvent);
    }
}
