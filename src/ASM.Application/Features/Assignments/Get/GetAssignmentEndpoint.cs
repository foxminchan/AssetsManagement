using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssignmentAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assignments.Get;

public sealed record GetAssignmentRequest(Guid Id);

public sealed class GetAssignmentEndpoint(ISender sender) : IEndpoint<Ok<AssignmentDto>, GetAssignmentRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/assignments/{id:guid}", async (Guid id) => await HandleAsync(new(id)))
            .Produces<Ok<AssignmentDto>>()
            .WithTags(nameof(Assignment))
            .WithName("Get Assignment")
            .RequireAuthorization(AuthRole.User);

    public async Task<Ok<AssignmentDto>> HandleAsync(GetAssignmentRequest request,
        CancellationToken cancellationToken = default)
    {
        GetAssignmentQuery query = new(request.Id);

        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result.Value.ToAssignmentDto());
    }
}
