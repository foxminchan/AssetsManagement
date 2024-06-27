using Ardalis.Specification;

namespace ASM.Application.Domain.AssignmentAggregate.Specifications;

public static class AssignmentExpression
{
    public static ISpecificationBuilder<Assignment> ApplyOrdering(this ISpecificationBuilder<Assignment> builder,
        string? orderBy, bool isDescending) =>
        orderBy switch
        {
            nameof(Assignment.Asset.AssetCode) => isDescending
                ? builder.OrderByDescending(x => x.Asset!.AssetCode)
                : builder.OrderBy(x => x.Asset!.AssetCode),
            nameof(Assignment.Asset.Name) => isDescending
                ? builder.OrderByDescending(x => x.Asset!.Name)
                : builder.OrderBy(x => x.Asset!.Name),
            nameof(Assignment.Staff.UserName) => isDescending
                ? builder.OrderByDescending(x => x.Staff!.StaffCode)
                : builder.OrderBy(x => x.Staff!.StaffCode),
            nameof(Assignment.UpdatedBy) => isDescending
                ? builder.OrderByDescending(x => x.UpdatedBy)
                : builder.OrderBy(x => x.UpdatedBy),
            nameof(Assignment.AssignedDate) => isDescending
                ? builder.OrderByDescending(x => x.AssignedDate)
                : builder.OrderBy(x => x.AssignedDate),
            nameof(Assignment.Asset.Category) => isDescending
                ? builder.OrderByDescending(x => x.Asset!.Category!.Name)
                : builder.OrderBy(x => x.Asset!.Category!.Name),
            nameof(Assignment.State) => isDescending
                ? builder.OrderByDescending(x => x.State)
                : builder.OrderBy(x => x.State),
            _ => isDescending
                ? builder.OrderByDescending(x => x.Asset!.AssetCode)
                : builder.OrderBy(x => x.Asset!.AssetCode),
        };

    public static ISpecificationBuilder<Assignment> ApplyPaging(this ISpecificationBuilder<Assignment> builder,
        int pageIndex, int pageSize) =>
        builder
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
}
