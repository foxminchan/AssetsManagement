using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Reports.AssetsByCategory;

public sealed record AssetsByCategoryRequest(string OrderBy, bool IsDescending);

public sealed class AssetsByCategoryEndpoint : IEndpoint<Ok<List<AssetsByCategoryDto>>, AssetsByCategoryRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/reports/assets-by-category", async (
                ISender sender,
                string orderBy = nameof(AssetsByCategoryDto.Category),
                bool isDescending = true) => await HandleAsync(new(orderBy, isDescending), sender))
            .Produces<Ok<List<AssetsByCategoryDto>>>()
            .WithTags("Reports")
            .WithName("Assets by Category")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok<List<AssetsByCategoryDto>>> HandleAsync(AssetsByCategoryRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        AssetsByCategoryQuery query = new(request.OrderBy, request.IsDescending);

        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result.Value.ToList());
    }
}
