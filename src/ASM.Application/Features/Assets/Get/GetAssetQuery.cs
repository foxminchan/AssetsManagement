using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Specifications;
using ASM.Application.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace ASM.Application.Features.Assets.Get;

public sealed record GetAssetQuery(Guid Id) : IQuery<Result<Asset>>;

public sealed class GetAssetHandler(IReadRepository<Asset> repository,
    IHttpContextAccessor httpContextAccessor)
    : IQueryHandler<GetAssetQuery, Result<Asset>>
{
    public async Task<Result<Asset>> Handle(GetAssetQuery request, CancellationToken cancellationToken)
    {
        var location = httpContextAccessor.HttpContext?.User.Claims
            .First(x => x.Type == nameof(Location)).Value;

        Guard.Against.NullOrEmpty(location);

        AssetFilterSpec spec = new(Enum.Parse<Location>(location), request.Id);

        var asset = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        Guard.Against.NotFound(request.Id, asset);

        return asset;
    }
}
