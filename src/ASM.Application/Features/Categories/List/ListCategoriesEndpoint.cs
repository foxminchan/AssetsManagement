using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssetAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Categories.List;

public sealed record ListCategoriesResponse(
    List<CategoryDto> Categories);

public sealed class ListCategoriesEndpoint : IEndpointWithoutRequest<Ok<ListCategoriesResponse>>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/categories", async (ISender sender) =>
                await HandleAsync(sender))
            .Produces<Ok<ListCategoriesResponse>>()
            .WithTags(nameof(Category))
            .WithName("List Categories")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok<ListCategoriesResponse>> HandleAsync(ISender sender,
        CancellationToken cancellationToken = default)
    {
        ListCategoriesQuery query = new();

        var result = await sender.Send(query, cancellationToken);

        ListCategoriesResponse response = new(result.ToCategoryDtos());

        return TypedResults.Ok(response);
    }
}
