using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using ASM.Application.Features.Assignments.UpdateState;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assignments.Accept;

public sealed class AcceptAssignmentEndpoint(ISender sender) : IEndpoint<Ok, Guid>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPatch("/assignments/{id:guid}/accepted",
                async (Guid id) => await HandleAsync(id))
            .Produces<Ok>()
            .Produces<NotFound<string>>()
            .WithTags(nameof(Assignment))
            .WithName("Accepted Assignment ")
            .RequireAuthorization(AuthRole.User);

    public async Task<Ok> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        UpdateAssignmentStateCommand command = new(id, State.Accepted);

        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }
}
