using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.Assignments.List;

public sealed record ListAssignmentsRequest(
    State? State,
    DateOnly? AssignedDate,
    int PageIndex,
    int PageSize,
    string? OrderBy,
    bool IsDescending,
    string? Search,
    Guid? AssetId);

public sealed record ListAssignmentsResponse(
    PagedInfo PagedInfo,
    List<AssignmentDto> Assignments);

public sealed class ListAssignmentsEndpoint
    : IEndpoint<Ok<ListAssignmentsResponse>, ListAssignmentsRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/assignments", async (
                    ISender sender,
                    int pageIndex = 1,
                    int pageSize = 20,
                    string? orderBy = nameof(Assignment.Asset.AssetCode),
                    bool isDescending = false,
                    State? state = null,
                    DateOnly? assignedDate = null,
                    string? search = null,
                    Guid? assetId = null) =>
                await HandleAsync(new(state, assignedDate, pageIndex, pageSize, orderBy, isDescending, search, assetId),
                    sender))
            .Produces<Ok<ListAssignmentsResponse>>()
            .WithTags(nameof(Assignment))
            .WithName("List Assignments")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok<ListAssignmentsResponse>> HandleAsync(ListAssignmentsRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        ListAssignmentsQuery query = new(
            request.State,
            request.AssignedDate,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.IsDescending,
            request.Search,
            request.AssetId);

        var result = await sender.Send(query, cancellationToken);

        var assignments = result.Value.ToAssignmentDtos(
            (int)result.PagedInfo.PageNumber,
            (int)result.PagedInfo.PageSize,
            (int)result.PagedInfo.TotalRecords,
            request.IsDescending);

        ListAssignmentsResponse response = new(result.PagedInfo, assignments);

        return TypedResults.Ok(response);
    }
}
