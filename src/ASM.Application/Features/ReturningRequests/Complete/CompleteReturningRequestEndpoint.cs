using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.ReturningRequestAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.ReturningRequests.Complete;

public sealed class CompleteReturningRequestEndpoint : IEndpoint<Ok, Guid>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPatch("/returning-requests/{id:guid}/complete",
                async (Guid id, ISender sender) => await HandleAsync(id, sender))
            .Produces<Ok>()
            .Produces<NotFound<string>>()
            .WithTags(nameof(ReturningRequest))
            .WithName("Complete Returning Request")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok> HandleAsync(Guid request, ISender sender, CancellationToken cancellationToken = default)
    {
        CompleteReturningRequestCommand command = new(request);

        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }
}
