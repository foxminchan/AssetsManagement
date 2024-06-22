using Ardalis.Specification;

namespace ASM.Application.Domain.IdentityAggregate.Specifications;

public static class StaffSpecExpression
{
    public static ISpecificationBuilder<Staff> ApplyOrdering(this ISpecificationBuilder<Staff> builder,
        string? orderBy, bool isDescending) =>
        orderBy switch
        {
            nameof(Staff.StaffCode) => isDescending
                ? builder.OrderByDescending(x => x.StaffCode)
                : builder.OrderBy(x => x.StaffCode),
            nameof(Staff.FullName) => isDescending
                ? builder.OrderByDescending(x => x.FullName)
                : builder.OrderBy(x => x.FullName),
            nameof(Staff.JoinedDate) => isDescending
                ? builder.OrderByDescending(x => x.JoinedDate)
                : builder.OrderBy(x => x.JoinedDate),
            nameof(Staff.UserName) => isDescending
                ? builder.OrderByDescending(x => x.UserName)
                : builder.OrderBy(x => x.UserName),
            _ => isDescending
                ? builder.OrderByDescending(x => x.RoleType)
                : builder.OrderBy(x => x.RoleType)
        };

    public static ISpecificationBuilder<Staff> ApplyPaging(this ISpecificationBuilder<Staff> builder,
        int pageIndex, int pageSize) =>
        builder
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
}
