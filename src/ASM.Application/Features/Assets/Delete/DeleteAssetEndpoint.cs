using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssetAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assets.Delete;

public sealed record DeleteAssetRequest(Guid Id);

public sealed class DeleteAssetEndpoint(ISender sender) : IEndpoint<NoContent, DeleteAssetRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapDelete("/assets/{id:guid}", async (Guid id) => await HandleAsync(new(id)))
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(nameof(Asset))
            .WithName("Delete Asset")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<NoContent> HandleAsync(DeleteAssetRequest request, CancellationToken cancellationToken = default)
    {
        DeleteAssetCommand command = new(request.Id);

        await sender.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }
}
