using Ardalis.Specification;
using ASM.Application.Domain.ReturningRequestAggregate.Enums;

namespace ASM.Application.Domain.ReturningRequestAggregate.Specifications;

public sealed class ReturningRequestFilterSpec : Specification<ReturningRequest>
{
    public ReturningRequestFilterSpec(
        State? state,
        DateOnly? returnedDate,
        int pageIndex,
        int pageSize,
        string? orderBy,
        bool isDescending,
        string? search)
    {
        if (state.HasValue) Query.Where(x => x.State == state);

        if (returnedDate.HasValue) Query.Where(x => x.ReturnedDate == returnedDate);

        if (!string.IsNullOrWhiteSpace(search))
            Query.Where(x =>
                x.Assignment!.Asset!.AssetCode!.Contains(search) ||
                x.Assignment!.Asset!.Name!.Contains(search) ||
                x.Assignment!.Staff!.Users!.First().UserName!.Contains(search));
        Query
            .ApplyOrdering(orderBy, isDescending)
            .ApplyPaging(pageIndex, pageSize);
    }
}
