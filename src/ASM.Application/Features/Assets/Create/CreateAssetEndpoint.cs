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

namespace ASM.Application.Features.Assets.Create;

public sealed record CreateAssetRequest(
    string Name,
    string Specification,
    DateOnly InstallDate,
    State State,
    Guid CategoryId);

public sealed class CreateAssetEndpoint : IEndpoint<Created<Guid>, CreateAssetRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/assets",
                async (CreateAssetRequest request, ISender sender) => await HandleAsync(request, sender))
            .Produces<Created<Guid>>(StatusCodes.Status201Created)
            .Produces<BadRequest<IEnumerable<ValidationError>>>(StatusCodes.Status400BadRequest)
            .WithTags(nameof(Asset))
            .WithName("Create Asset")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Created<Guid>> HandleAsync(CreateAssetRequest request,
        ISender sender,
        CancellationToken cancellationToken = default)
    {
        CreateAssetCommand command = new(
            request.Name,
            request.Specification,
            request.InstallDate,
            request.State,
            request.CategoryId);

        var result = await sender.Send(command, cancellationToken);

        return TypedResults.Created($"/api/assets/{result.Value}", result.Value);
    }
}
