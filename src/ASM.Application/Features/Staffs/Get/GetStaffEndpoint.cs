using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Staffs.Get;

public sealed record GetStaffRequest(Guid Id);

public sealed class GetStaffEndpoint : IEndpoint<Ok<StaffDto>, GetStaffRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/users/{id:guid}", async (Guid id, ISender sender) => await HandleAsync(new(id), sender))
            .Produces<Ok<StaffDto>>()
            .Produces<NotFound<string>>(StatusCodes.Status404NotFound)
            .WithTags(nameof(Staff))
            .WithName("Get User")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok<StaffDto>> HandleAsync(GetStaffRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        GetStaffQuery query = new(request.Id);

        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result.Value.ToStaffDto());
    }
}
