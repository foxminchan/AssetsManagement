using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.IdentityAggregate.Specifications;
using ASM.Application.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace ASM.Application.Features.Staffs.List;

public sealed record ListStaffsQuery(
    RoleType? RoleType,
    int PageIndex,
    int PageSize,
    string? OrderBy,
    bool IsDescending,
    string? Search,
    Guid? FeaturedStaffId) : IQuery<PagedResult<IEnumerable<Staff>>>;

public sealed class ListStaffHandler(
    IReadRepository<Staff> repository,
    IHttpContextAccessor httpContextAccessor)
    : IQueryHandler<ListStaffsQuery, PagedResult<IEnumerable<Staff>>>
{
    public async Task<PagedResult<IEnumerable<Staff>>> Handle(ListStaffsQuery request,
        CancellationToken cancellationToken)
    {
        var location = httpContextAccessor.HttpContext?.User.Claims
            .First(x => x.Type == nameof(Location)).Value;

        Guard.Against.NullOrEmpty(location);

        StaffFilterSpec spec = new(
            Enum.Parse<Location>(location),
            request.RoleType,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.IsDescending,
            request.Search,
            request.FeaturedStaffId);

        var staffs = await repository.ListAsync(spec, cancellationToken);

        var totalRecords = await repository.CountAsync(spec, cancellationToken);

        var totalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize);

        PagedInfo pagedInfo = new(request.PageIndex, request.PageSize, totalPages, totalRecords);

        return new(pagedInfo, staffs);
    }
}
