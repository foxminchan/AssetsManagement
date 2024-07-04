using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssetAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Categories.Create;

public sealed record CreateCategoryRequest(string Name, string Prefix);

public sealed class CreateCategoryEndpoint : IEndpoint<Created<Guid>, CreateCategoryRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/categories",
                async (CreateCategoryRequest request, ISender sender) => await HandleAsync(request, sender))
            .Produces<Created<Guid>>(StatusCodes.Status201Created)
            .Produces<BadRequest<IEnumerable<ValidationError>>>(StatusCodes.Status400BadRequest)
            .Produces<Conflict<ValidationError>>(StatusCodes.Status409Conflict)
            .WithTags(nameof(Category))
            .WithName("Create Category")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Created<Guid>> HandleAsync(CreateCategoryRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        CreateCategoryCommand command = new(request.Name, request.Prefix);

        var result = await sender.Send(command, cancellationToken);

        return TypedResults.Created($"/api/categories/{result.Value}", result.Value);
    }
}
