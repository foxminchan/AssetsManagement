using Ardalis.Specification;

namespace ASM.Application.Domain.AssetAggregate.Specifications;

public static class AssetSpecExpression
{
    public static ISpecificationBuilder<Asset> ApplyOrdering(this ISpecificationBuilder<Asset> builder,
        string? orderBy, bool isDescending) =>
        orderBy switch
        {
            nameof(Asset.AssetCode) => isDescending
                ? builder.OrderByDescending(x => x.AssetCode)
                : builder.OrderBy(x => x.AssetCode),
            nameof(Asset.Name) => isDescending
                ? builder.OrderByDescending(x => x.Name)
                : builder.OrderBy(x => x.Name),
            nameof(Asset.Category) => isDescending
                ? builder.OrderByDescending(x => x.Category!.Name)
                : builder.OrderBy(x => x.Category!.Name),
            nameof(Asset.State) => isDescending
                ? builder.OrderByDescending(x => x.State)
                : builder.OrderBy(x => x.State),
            _ => isDescending
                ? builder.OrderByDescending(x => x.AssetCode)
                : builder.OrderBy(x => x.AssetCode)
        };

    public static ISpecificationBuilder<Asset> ApplyPaging(this ISpecificationBuilder<Asset> builder,
        int pageIndex, int pageSize) =>
        builder
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
}
