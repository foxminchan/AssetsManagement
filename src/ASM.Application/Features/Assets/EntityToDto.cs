using ASM.Application.Domain.AssetAggregate;

namespace ASM.Application.Features.Assets;

public static class EntityToDto
{
    public static AssetDto ToAssetDto(this Asset asset) =>
        new(asset.Id,
            asset.Name,
            asset.AssetCode,
            asset.Specification,
            asset.Category?.Name,
            asset.InstallDate,
            asset.State,
            asset.Location,
            asset.CategoryId);

    public static List<AssetDto> ToAssetDtos(this IEnumerable<Asset> assets) => assets.Select(ToAssetDto).ToList();
}
