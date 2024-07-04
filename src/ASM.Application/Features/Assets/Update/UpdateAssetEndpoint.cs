using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assets.Update;

public sealed record UpdateAssetRequest(
    Guid Id,
    string? Name,
    string? Specification,
    DateOnly InstalledDate,
    State State);

public sealed class UpdateAssetEndpoint : IEndpoint<Ok, UpdateAssetRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPut("/assets",
                async (UpdateAssetRequest request, ISender sender) => await HandleAsync(request, sender))
            .Produces<Ok>()
            .Produces<BadRequest<IEnumerable<ValidationError>>>(StatusCodes.Status400BadRequest)
            .WithTags(nameof(Asset))
            .WithName("Update Asset")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok> HandleAsync(UpdateAssetRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        UpdateAssetCommand command = new(
            request.Id,
            request.Name,
            request.Specification,
            request.InstalledDate,
            request.State);

        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }
}
