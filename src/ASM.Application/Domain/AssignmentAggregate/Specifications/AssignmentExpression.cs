using Ardalis.Specification;

namespace ASM.Application.Domain.AssignmentAggregate.Specifications;

public static class AssignmentExpression
{
    public static IOrderedSpecificationBuilder<Assignment> ApplyPrimaryOrdering(this ISpecificationBuilder<Assignment> builder,
        Guid? featuredAssignmentId) => builder.OrderBy(x => x.Id == featuredAssignmentId ? 0 : 1);
    public static ISpecificationBuilder<Assignment> ApplySecondaryOrdering(this IOrderedSpecificationBuilder<Assignment> builder,
        string? orderBy, bool isDescending) =>
        orderBy switch
        {
            nameof(Assignment.Asset.AssetCode) => isDescending
                ? builder.ThenByDescending(x => x.Asset!.AssetCode)
                : builder.ThenBy(x => x.Asset!.AssetCode),
            nameof(Assignment.Asset.Name) => isDescending
                ? builder.ThenByDescending(x => x.Asset!.Name)
                : builder.ThenBy(x => x.Asset!.Name),
            nameof(Assignment.Staff.UserName) => isDescending
                ? builder.ThenByDescending(x => x.Staff!.StaffCode)
                : builder.ThenBy(x => x.Staff!.StaffCode),
            nameof(Assignment.UpdatedBy) => isDescending
                ? builder.ThenByDescending(x => x.UpdatedBy)
                : builder.ThenBy(x => x.UpdatedBy),
            nameof(Assignment.AssignedDate) => isDescending
                ? builder.ThenByDescending(x => x.AssignedDate)
                : builder.ThenBy(x => x.AssignedDate),
            nameof(Assignment.Asset.Category) => isDescending
                ? builder.ThenByDescending(x => x.Asset!.Category!.Name)
                : builder.ThenBy(x => x.Asset!.Category!.Name),
            nameof(Assignment.State) => isDescending
                ? builder.ThenByDescending(x => x.State)
                : builder.ThenBy(x => x.State),
            _ => isDescending
                ? builder.ThenByDescending(x => x.Asset!.AssetCode)
                : builder.ThenBy(x => x.Asset!.AssetCode),
        };

    public static ISpecificationBuilder<Assignment> ApplyPaging(this ISpecificationBuilder<Assignment> builder,
        int pageIndex, int pageSize) =>
        builder
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
}
