using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assignments.UpdateState;

public sealed record UpdateAssignmentStateRequest(Guid Id, State State);

public sealed class UpdateAssignmentStateEndpoint(ISender sender) : IEndpoint<Ok, UpdateAssignmentStateRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPatch("/assignments",
                async (UpdateAssignmentStateRequest request) => await HandleAsync(request))
            .Produces<Ok>()
            .Produces<NotFound<string>>()
            .WithTags(nameof(Assignment))
            .WithName("Update Assignment State")
            .RequireAuthorization(AuthRole.User);

    public async Task<Ok> HandleAsync(UpdateAssignmentStateRequest request,
        CancellationToken cancellationToken = default)
    {
        UpdateAssignmentStateCommand command = new(request.Id, request.State);

        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }
}
