using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssignmentAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assignments.GetOwn;

public sealed record GetOwnAssignmentRequest(Guid Id);

public sealed class GetOwnAssignmentEndpoint : IEndpoint<Ok<AssignmentDto>, GetOwnAssignmentRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/assignments/own/{id:guid}",
                async (Guid id, ISender sender) => await HandleAsync(new(id), sender))
            .Produces<Ok<AssignmentDto>>()
            .Produces<NotFound<string>>(StatusCodes.Status404NotFound)
            .WithTags(nameof(Assignment))
            .WithName("Get Own Assignment")
            .RequireAuthorization(AuthRole.User);

    public async Task<Ok<AssignmentDto>> HandleAsync(GetOwnAssignmentRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        GetOwnAssignmentQuery query = new(request.Id);

        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result.Value.ToAssignmentDto());
    }
}
