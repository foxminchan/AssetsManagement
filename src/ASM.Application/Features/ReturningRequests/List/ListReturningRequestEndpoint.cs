using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Endpoints;
using ASM.Application.Domain.ReturningRequestAggregate;
using ASM.Application.Domain.ReturningRequestAggregate.Enums;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Features.ReturningRequests.List;

public sealed record ListReturningRequestRequest(
    State? State,
    DateOnly? ReturnedDate,
    int PageIndex,
    int PageSize,
    string? OrderBy,
    bool IsDescending,
    string? Search);

public sealed record ListReturningRequestResponse(
    PagedInfo PagedInfo,
    List<ReturningRequestDto> ReturningRequests);

public sealed class
    ListReturningRequestEndpoint : IEndpoint<Ok<ListReturningRequestResponse>, ListReturningRequestRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/returning-requests", async (
                    ISender sender,
                    int pageIndex = 1,
                    int pageSize = 20,
                    string? orderBy = nameof(ReturningRequest.Assignment.Asset.AssetCode),
                    bool isDescending = false,
                    State? state = null,
                    DateOnly? returnedDate = null,
                    string? search = null) =>
                await HandleAsync(new(state, returnedDate, pageIndex, pageSize, orderBy, isDescending, search),
                    sender))
            .Produces<Ok<ListReturningRequestResponse>>()
            .WithTags(nameof(ReturningRequest))
            .WithName("List Returning Requests")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok<ListReturningRequestResponse>> HandleAsync(ListReturningRequestRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        ListReturningRequestQuery query = new(
            request.State,
            request.ReturnedDate,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.IsDescending,
            request.Search);

        var result = await sender.Send(query, cancellationToken);

        var returningRequests = result.Value.ToReturningRequestDtos(
            (int)result.PagedInfo.PageNumber,
            (int)result.PagedInfo.PageSize,
            (int)result.PagedInfo.TotalRecords,
            request.IsDescending);

        var response = new ListReturningRequestResponse(result.PagedInfo, returningRequests);

        return TypedResults.Ok(response);
    }
}
