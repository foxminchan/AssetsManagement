using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Staffs.Create;

public sealed record CreateStaffRequest(
    string FirstName,
    string LastName,
    DateOnly Dob,
    DateOnly JoinedDate,
    Gender Gender,
    RoleType RoleType);

public sealed class CreateStaffEndpoint(ISender sender)
    : IEndpoint<Created<Guid>, CreateStaffRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/users", async (CreateStaffRequest request) => await HandleAsync(request))
            .Produces<Created<Guid>>(StatusCodes.Status201Created)
            .Produces<BadRequest<IEnumerable<ValidationError>>>(StatusCodes.Status400BadRequest)
            .WithTags(nameof(Staff))
            .WithName("Create User")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Created<Guid>> HandleAsync(CreateStaffRequest request,
        CancellationToken cancellationToken = default)
    {
        CreateStaffCommand command = new(
            request.FirstName,
            request.LastName,
            request.Dob,
            request.JoinedDate,
            request.Gender,
            request.RoleType);

        var result = await sender.Send(command, cancellationToken);

        return TypedResults.Created($"/api/users/{result.Value}", result.Value);
    }
}
