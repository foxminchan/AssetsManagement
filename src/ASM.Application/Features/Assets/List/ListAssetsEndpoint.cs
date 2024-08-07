﻿using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assets.List;

public sealed record ListAssetRequest(
    string[]? Categories,
    State[]? State,
    int PageIndex,
    int PageSize,
    string? OrderBy,
    bool IsDescending,
    string? Search,
    Guid? FeaturedAssetId);

public sealed record ListAssetResponse(
    PagedInfo PagedInfo,
    List<AssetDto> Assets);

public sealed class ListAssetsEndpoint : IEndpoint<Ok<ListAssetResponse>, ListAssetRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/assets", async (
                    ISender sender,
                    string[]? categories = null,
                    State[]? state = null,
                    int pageIndex = 1,
                    int pageSize = 20,
                    string? orderBy = nameof(Asset.AssetCode),
                    bool isDescending = false,
                    string? search = null,
                    Guid? featuredAssetId = null) =>
                await HandleAsync(new(categories, state, pageIndex, pageSize, orderBy, isDescending, search, featuredAssetId), sender))
            .Produces<Ok<ListAssetResponse>>()
            .WithTags(nameof(Asset))
            .WithName("List Assets")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok<ListAssetResponse>> HandleAsync(ListAssetRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        ListAssetsQuery query = new(
            request.Categories,
            request.State,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.IsDescending,
            request.Search,
            request.FeaturedAssetId);

        var result = await sender.Send(query, cancellationToken);

        ListAssetResponse response = new(result.PagedInfo, result.Value.ToAssetDtos());

        return TypedResults.Ok(response);
    }
}
