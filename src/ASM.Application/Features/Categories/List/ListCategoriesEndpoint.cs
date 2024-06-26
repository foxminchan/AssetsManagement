using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssetAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Categories.List;

public sealed record ListCategorieResponse(
    List<CategoryDto> Categories);

public sealed class ListCategoriesEndpoint(ISender sender) : IEndpointWithoutRequest<Ok<ListCategorieResponse>>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/categories", async () =>
                await HandleAsync())
            .Produces<Ok<ListCategorieResponse>>()
            .WithTags(nameof(Category))
            .WithName("List Categories")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok<ListCategorieResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        ListCategoriesQuery query = new();

        var result = await sender.Send(query, cancellationToken);

        ListCategorieResponse response = new(result.ToCategoryDtos());

        return TypedResults.Ok(response);
    }
}
