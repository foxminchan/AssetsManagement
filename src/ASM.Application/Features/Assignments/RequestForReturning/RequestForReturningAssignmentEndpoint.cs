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

namespace ASM.Application.Features.Assignments.RequestForReturning;

public sealed class RequestForReturningAssignmentEndpoint : IEndpoint<Ok, Guid>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPatch("/assignments/{id:guid}/request-for-returning",
                async (Guid id, ISender sender) => await HandleAsync(id, sender))
            .Produces<Ok>()
            .Produces<NotFound<string>>()
            .WithTags(nameof(Assignment))
            .WithName("Waiting For Returning Assignment")
            .RequireAuthorization(AuthRole.User);

    public async Task<Ok> HandleAsync(Guid id, ISender sender, CancellationToken cancellationToken = default)
    {
        UpdateAssignmentStateCommand command = new(id, State.RequestForReturning);

        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }
}
