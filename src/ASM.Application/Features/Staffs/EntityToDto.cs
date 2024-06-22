using ASM.Application.Domain.IdentityAggregate;

namespace ASM.Application.Features.Staffs;

public static class EntityToDto
{
    public static StaffDto ToStaffDto(this Staff staff) =>
        new(staff.Id,
            staff.FirstName,
            staff.LastName,
            staff.FullName,
            staff.StaffCode,
            staff.UserName,
            staff.JoinedDate,
            staff.Dob,
            staff.RoleType,
            staff.Gender,
            staff.Location);

    public static List<StaffDto> ToStaffDtos(this IEnumerable<Staff> staffs) => staffs.Select(ToStaffDto).ToList();
}
