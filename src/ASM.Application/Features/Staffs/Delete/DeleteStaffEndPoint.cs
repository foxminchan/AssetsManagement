using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Staffs.Delete;

public sealed record DeleteStaffRequest(Guid UserId);


public sealed class DeleteStaffEndPoint(ISender sender) : IEndpoint<NoContent, DeleteStaffRequest>
{

    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapDelete("/users/{id:guid}", async (Guid id) => await HandleAsync(new(id)))
             .Produces<NoContent>(StatusCodes.Status204NoContent)
             .Produces<BadRequest<IEnumerable<ValidationError>>>(StatusCodes.Status400BadRequest)
             .WithTags(nameof(Staff))
             .WithName("Delete Staff")
             .RequireAuthorization(AuthRole.User);


    public async Task<NoContent> HandleAsync(DeleteStaffRequest request, CancellationToken cancellationToken = default)
    {
        var command = new DeleteStaffCommand(request.UserId);
        await sender.Send(command, cancellationToken);
        return TypedResults.NoContent();
    }
}
