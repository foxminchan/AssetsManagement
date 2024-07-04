using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssignmentAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assignments.Delete;

public sealed record DeleteAssignmentRequest(Guid Id);

public sealed class DeleteAssignmentEndpoint : IEndpoint<NoContent, DeleteAssignmentRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapDelete("/assignments/{id:guid}",
                async (Guid id, ISender sender) => await HandleAsync(new(id), sender))
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound<string>>(StatusCodes.Status404NotFound)
            .WithTags(nameof(Assignment))
            .WithName("Delete Assignment")
            .RequireAuthorization(AuthRole.User);

    public async Task<NoContent> HandleAsync(DeleteAssignmentRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        DeleteAssignmentCommand command = new(request.Id);

        await sender.Send(command, cancellationToken);

        return TypedResults.NoContent();
    }
}
