using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Users.UpdatePassword;

public sealed record UpdatePasswordRequest(Guid Id, string OldPassword, string NewPassword);

public sealed class UpdatePasswordEndpoint(ISender sender)
    : IEndpoint<Ok, UpdatePasswordRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPatch("/updatePassword", async (UpdatePasswordRequest request) => await HandleAsync(request))
            .Produces<Ok>()
            .Produces<BadRequest<IEnumerable<ValidationError>>>(StatusCodes.Status400BadRequest)
            .WithTags(nameof(ApplicationUser))
            .WithName("Update Password")
            .RequireAuthorization(AuthRole.User);

    public async Task<Ok> HandleAsync(UpdatePasswordRequest request, CancellationToken cancellationToken = default)
    {
        UpdatePasswordCommand command = new(request.Id, request.OldPassword, request.NewPassword);

        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }
}
