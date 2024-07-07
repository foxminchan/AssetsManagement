using Ardalis.Specification;

namespace ASM.Application.Domain.IdentityAggregate.Specifications;

public static class StaffSpecExpression
{
    public static IOrderedSpecificationBuilder<Staff> ApplyPrimaryOrdering(this ISpecificationBuilder<Staff> builder,
        Guid? featuredStaffId) => builder.OrderBy(x => x.Id == featuredStaffId ? 0 : 1);
    public static ISpecificationBuilder<Staff> ApplySecondaryOrdering(this IOrderedSpecificationBuilder<Staff> builder,
        string? orderBy, bool isDescending) =>
        orderBy switch
        {
            nameof(Staff.StaffCode) => isDescending
                ? builder.ThenByDescending(x => x.StaffCode)
                : builder.ThenBy(x => x.StaffCode),
            nameof(Staff.FullName) => isDescending
                ? builder.ThenByDescending(x => x.FirstName + " " + x.LastName)
                : builder.ThenBy(x => x.FirstName + " " + x.LastName),
            nameof(Staff.JoinedDate) => isDescending
                ? builder.ThenByDescending(x => x.JoinedDate)
                : builder.ThenBy(x => x.JoinedDate),
            nameof(Staff.UserName) => isDescending
                ? builder.ThenByDescending(x => x.Users!.First().UserName)
                : builder.ThenBy(x => x.Users!.First().UserName),
            _ => isDescending
                ? builder.ThenByDescending(x => x.RoleType)
                : builder.ThenBy(x => x.RoleType)
        };

    public static ISpecificationBuilder<Staff> ApplyPaging(this ISpecificationBuilder<Staff> builder,
        int pageIndex, int pageSize) =>
        builder
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
}
