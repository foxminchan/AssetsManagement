using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssetAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assets.Get;

public sealed record GetAssetRequest(Guid Id);

public sealed class GetAssetEndpoint(ISender sender) : IEndpoint<Ok<AssetDto>, GetAssetRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/assets/{id:guid}", async (Guid id) => await HandleAsync(new(id)))
            .Produces<Ok<AssetDto>>()
            .Produces<NotFound<string>>(StatusCodes.Status404NotFound)
            .WithTags(nameof(Asset))
            .WithName("Get Asset")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok<AssetDto>> HandleAsync(GetAssetRequest request, CancellationToken cancellationToken = default)
    {
        GetAssetQuery query = new(request.Id);

        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result.Value.ToAssetDto());
    }
}
