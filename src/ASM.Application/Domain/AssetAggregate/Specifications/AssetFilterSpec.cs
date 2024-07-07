using Ardalis.Specification;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.Shared;

namespace ASM.Application.Domain.AssetAggregate.Specifications;

public sealed class AssetFilterSpec : Specification<Asset>
{
    public AssetFilterSpec(
        Location location,
        string[]? categories,
        State[]? state,
        int pageIndex,
        int pageSize,
        string? orderBy,
        bool isDescending,
        string? search,
        Guid? featuredAssetId)
    {
        Query.Where(x => x.Location == location);

        if (categories?.Length != 0)
            Query.Where(x => categories!.Contains(x.Category!.Name) || x.Id == featuredAssetId);

        if (state!.Length != 0)
            Query.Where(x => state.Contains(x.State) || x.Id == featuredAssetId);

        if (!string.IsNullOrEmpty(search))
            Query.Where(x => x.AssetCode!.Contains(search) || x.Name!.Contains(search) || x.Id == featuredAssetId);

        Query
            .ApplyPrimaryOrdering(featuredAssetId)
            .ApplySecondaryOrdering(orderBy, isDescending)
            .ApplyPaging(pageIndex, pageSize);
    }

    public AssetFilterSpec(Location location, Guid id) => Query.Where(x => x.Id == id && x.Location == location);
}
