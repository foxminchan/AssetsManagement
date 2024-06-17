using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;
using ASM.Application.Domain.IdentityAggregate.Enums;

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
    public ICollection<ApplicationUser>?  Users { get; set; } = [];

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

    public void Update(DateOnly dob, DateOnly joinedDate, Gender gender, RoleType roleType,
        Location location)
    {
        Dob = dob;
        JoinedDate = joinedDate;
        Gender = Guard.Against.EnumOutOfRange(gender);
        RoleType = Guard.Against.EnumOutOfRange(roleType);
        Location = Guard.Against.EnumOutOfRange(location);
    }
}
