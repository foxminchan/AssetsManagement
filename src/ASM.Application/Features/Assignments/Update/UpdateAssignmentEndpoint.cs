using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssignmentAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assignments.Update;

public sealed record UpdateAssignmentRequest(
    Guid Id,
    Guid AssetId,
    Guid UserId,
    DateOnly AssignedDate,
    string Note);

public sealed class UpdateAssignmentEndpoint : IEndpoint<Ok, UpdateAssignmentRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPut("/assignments",
                async (UpdateAssignmentRequest request, ISender sender) => await HandleAsync(request, sender))
            .Produces<Ok>()
            .Produces<BadRequest<IEnumerable<ValidationError>>>(StatusCodes.Status400BadRequest)
            .WithTags(nameof(Assignment))
            .WithName("Update Assignment")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok> HandleAsync(UpdateAssignmentRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        UpdateAssignmentCommand command = new(request.Id, request.AssetId, request.UserId, request.AssignedDate,
            request.Note);
        await sender.Send(command, cancellationToken);
        return TypedResults.Ok();
    }
}
