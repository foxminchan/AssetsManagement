using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace ASM.Application.Features.Staffs.Update;

public sealed record UpdateStaffRequest(
    Guid Id,
    DateOnly Dob,
    DateOnly JoinedDate,
    Gender Gender,
    RoleType RoleType);

public sealed class UpdateStaffEndpoint(ISender sender) : IEndpoint<Ok, UpdateStaffRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPut("/users", async (UpdateStaffRequest request) => await HandleAsync(request))
            .Produces<Ok>()
            .Produces<BadRequest<IEnumerable<ValidationError>>>(StatusCodes.Status400BadRequest)
            .WithTags(nameof(Staff))
            .WithName("Update Staff")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok> HandleAsync(UpdateStaffRequest request, CancellationToken cancellationToken = default)
    {
        UpdateStaffCommand command = new(request.Id, request.Dob, request.JoinedDate, request.Gender, request.RoleType);

        await sender.Send(command, cancellationToken);

        return TypedResults.Ok();
    }
}