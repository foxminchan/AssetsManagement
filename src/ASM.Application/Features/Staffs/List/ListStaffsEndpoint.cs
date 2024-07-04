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

namespace ASM.Application.Features.Staffs.List;

public sealed record ListStaffRequest(
    RoleType? RoleType,
    int PageIndex,
    int PageSize,
    string? OrderBy,
    bool IsDescending,
    string? Search);

public sealed record ListStaffResponse(
    PagedInfo PagedInfo,
    List<StaffDto> Users);

public sealed class ListStaffsEndpoint : IEndpoint<Ok<ListStaffResponse>, ListStaffRequest>
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/users", async (
                    ISender sender,
                    int pageIndex = 1,
                    int pageSize = 20,
                    string? orderBy = nameof(Staff.StaffCode),
                    bool isDescending = false,
                    RoleType? roleType = null,
                    string? search = null) =>
                await HandleAsync(new(roleType, pageIndex, pageSize, orderBy, isDescending, search), sender))
            .Produces<Ok<ListStaffResponse>>()
            .WithTags(nameof(Staff))
            .WithName("List Staffs")
            .RequireAuthorization(AuthRole.Admin);

    public async Task<Ok<ListStaffResponse>> HandleAsync(ListStaffRequest request, ISender sender,
        CancellationToken cancellationToken = default)
    {
        ListStaffsQuery query = new(
            request.RoleType,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.IsDescending,
            request.Search);

        var result = await sender.Send(query, cancellationToken);

        ListStaffResponse response = new(result.PagedInfo, result.Value.ToStaffDtos());

        return TypedResults.Ok(response);
    }
}
