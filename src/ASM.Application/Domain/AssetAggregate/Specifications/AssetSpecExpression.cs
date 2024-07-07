using Ardalis.Specification;

namespace ASM.Application.Domain.AssetAggregate.Specifications;

public static class AssetSpecExpression
{
    public static IOrderedSpecificationBuilder<Asset> ApplyPrimaryOrdering(this ISpecificationBuilder<Asset> builder,
        Guid? featureAssetId) => builder.OrderBy(x => x.Id == featureAssetId ? 0 : 1);
    public static ISpecificationBuilder<Asset> ApplySecondaryOrdering(this IOrderedSpecificationBuilder<Asset> builder,
        string? orderBy, bool isDescending) =>
        orderBy switch
        {
            nameof(Asset.AssetCode) => isDescending
                ? builder.ThenByDescending(x => x.AssetCode)
                : builder.ThenBy(x => x.AssetCode),
            nameof(Asset.Name) => isDescending
                ? builder.ThenByDescending(x => x.Name)
                : builder.ThenBy(x => x.Name),
            nameof(Asset.Category) => isDescending
                ? builder.ThenByDescending(x => x.Category!.Name)
                : builder.ThenBy(x => x.Category!.Name),
            nameof(Asset.State) => isDescending
                ? builder.ThenByDescending(x => x.State)
                : builder.ThenBy(x => x.State),
            _ => isDescending
                ? builder.ThenByDescending(x => x.AssetCode)
                : builder.ThenBy(x => x.AssetCode)
        };

    public static ISpecificationBuilder<Asset> ApplyPaging(this ISpecificationBuilder<Asset> builder,
        int pageIndex, int pageSize) =>
        builder
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
}
