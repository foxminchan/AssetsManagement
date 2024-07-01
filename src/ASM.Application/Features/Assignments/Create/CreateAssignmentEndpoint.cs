using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssignmentAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assignments.Create;

public sealed record CreateAssignmentRequest(
    Guid UserId,
    Guid AssetId,
    DateOnly AssignedDate,
    string Note);

public sealed class CreateAssignmentEndpoint(ISender sender)
    : IEndpoint<Created<Guid>, CreateAssignmentRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/assignments", async (CreateAssignmentRequest request) => await HandleAsync(request))
            .Produces<Created<Guid>>(StatusCodes.Status201Created)
            .Produces<BadRequest<IEnumerable<ValidationError>>>(StatusCodes.Status400BadRequest)
            .WithTags(nameof(Assignment))
            .WithName("Create Assignment")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Created<Guid>> HandleAsync(CreateAssignmentRequest request,
        CancellationToken cancellationToken = default)
    {
        CreateAssignmentCommand command = new(
            request.UserId,
            request.AssetId,
            request.AssignedDate,
            request.Note);

        var result = await sender.Send(command, cancellationToken);

        return TypedResults.Created($"/api/assignments/{result.Value}", result.Value);
    }
}
