using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssignmentAggregate;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assignments.ListOwn;

public sealed record ListOwnAssignmentsRequest(string? OrderBy, bool IsDescending);

public sealed class ListOwnAssignmentsEndpoint
    : IEndpoint<Ok<List<AssignmentDto>>, ListOwnAssignmentsRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/assignments/own", async (
                    ISender sender,
                    string? orderBy = nameof(Assignment.Asset.AssetCode),
                    bool isDescending = false) =>
                await HandleAsync(new(orderBy, isDescending), sender))
            .Produces<Ok<List<AssignmentDto>>>()
            .WithTags(nameof(Assignment))
            .WithName("List Own Assignments")
            .RequireAuthorization(AuthRole.User);

    public async Task<Ok<List<AssignmentDto>>> HandleAsync(ListOwnAssignmentsRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        ListOwnAssignmentsQuery query = new(request.OrderBy, request.IsDescending);

        var result = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(result.ToAssignmentDtos());
    }
}
