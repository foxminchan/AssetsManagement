﻿using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssignmentAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assignments.Get;

public sealed record GetAssignmentRequest(Guid Id);

public sealed class GetAssignmentEndpoint : IEndpoint<Ok<AssignmentDto>, GetAssignmentRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/assignments/{id:guid}", async (Guid id, ISender sender) => await HandleAsync(new(id), sender))
            .Produces<Ok<AssignmentDto>>()
            .Produces<NotFound<string>>(StatusCodes.Status404NotFound)
            .WithTags(nameof(Assignment))
            .WithName("Get Assignment")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok<AssignmentDto>> HandleAsync(GetAssignmentRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        GetAssignmentQuery query = new(request.Id);

        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result.Value.ToAssignmentDto());
    }
}
