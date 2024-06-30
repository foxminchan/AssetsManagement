using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.AssetAggregate.Specifications;
using ASM.Application.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace ASM.Application.Features.Assets.List;

public sealed record ListAssetsQuery(
    Guid? CategoryId,
    State[]? State,
    int PageIndex,
    int PageSize,
    string? OrderBy,
    bool IsDescending,
    string? Search) : IQuery<PagedResult<IEnumerable<Asset>>>;

public sealed class ListAssetsHandler(
    IReadRepository<Asset> repository,
    IHttpContextAccessor httpContextAccessor)
    : IQueryHandler<ListAssetsQuery, PagedResult<IEnumerable<Asset>>>
{
    public async Task<PagedResult<IEnumerable<Asset>>> Handle(ListAssetsQuery request,
        CancellationToken cancellationToken)
    {
        var location = httpContextAccessor.HttpContext?.User.Claims
            .First(x => x.Type == nameof(Location)).Value;

        Guard.Against.NullOrEmpty(location);

        AssetFilterSpec spec = new(
            Enum.Parse<Location>(location),
            request.CategoryId,
            request.State,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.IsDescending,
            request.Search);

        var assets = await repository.ListAsync(spec, cancellationToken);

        var totalRecords = await repository.CountAsync(spec, cancellationToken);

        var totalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize);

        PagedInfo pagedInfo = new(request.PageIndex, request.PageSize, totalPages, totalRecords);

        return new(pagedInfo, assets);
    }
}
