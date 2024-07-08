using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Specifications;
using ASM.Application.Domain.ReturningRequestAggregate;
using ASM.Application.Domain.ReturningRequestAggregate.Enums;
using ASM.Application.Domain.ReturningRequestAggregate.Specifications;

namespace ASM.Application.Features.ReturningRequests.List;

public sealed record ListReturningRequestQuery(
    State? State,
    DateOnly? ReturnedDate,
    int PageIndex,
    int PageSize,
    string? OrderBy,
    bool IsDescending,
    string? Search) : IQuery<PagedResult<IEnumerable<ReturningRequest>>>;

public sealed class ListReturningRequestHandler(
    IReadRepository<ReturningRequest> returningRequestRepository,
    IReadRepository<Staff> staffRepository)
    : IQueryHandler<ListReturningRequestQuery, PagedResult<IEnumerable<ReturningRequest>>>
{
    public async Task<PagedResult<IEnumerable<ReturningRequest>>> Handle(ListReturningRequestQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new ReturningRequestFilterSpec(
            request.State,
            request.ReturnedDate,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.IsDescending,
            request.Search);

        var returningRequests = await returningRequestRepository.ListAsync(spec, cancellationToken);

        returningRequests.ForEach(returningRequest =>
            returningRequest.AcceptedBy = returningRequest.Staff?.Users?.First().UserName);

        var staffIds = returningRequests.Select(a => a.CreatedBy).Distinct();
        var staffDictionary = (await staffRepository.ListAsync(new StaffFilterSpec(staffIds), cancellationToken))
            .ToDictionary(staff => staff.Id);

        foreach (var returningRequest in returningRequests)
        {
            staffDictionary.TryGetValue(returningRequest.CreatedBy, out var acceptedBy);
            returningRequest.RequestedBy = acceptedBy?.UserName;
        }

        returningRequests = OrderByNotMappedProperties(returningRequests, request.OrderBy, request.IsDescending);

        var totalRecords = await returningRequestRepository.CountAsync(spec, cancellationToken);

        var totalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize);

        PagedInfo pagedInfo = new(request.PageIndex, request.PageSize, totalPages, totalRecords);

        return new(pagedInfo, returningRequests);
    }

    private static List<ReturningRequest> OrderByNotMappedProperties(
        List<ReturningRequest> returningRequests,
        string? orderBy,
        bool isDescending) =>
        orderBy switch
        {
            nameof(ReturningRequest.RequestedBy) => isDescending
                ? [.. returningRequests.OrderByDescending(x => x.RequestedBy)]
                : [.. returningRequests.OrderBy(x => x.RequestedBy)],
            nameof(ReturningRequest.AcceptBy) => isDescending
                ? [.. returningRequests.OrderByDescending(x => x.AcceptedBy)]
                : [.. returningRequests.OrderBy(x => x.AcceptedBy)],
            _ => returningRequests
        };
}
