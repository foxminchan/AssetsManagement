using Ardalis.Specification;
using ASM.Application.Domain.AssignmentAggregate.Enums;

namespace ASM.Application.Domain.AssignmentAggregate.Specifications;

public sealed class AssignmentFilterSpec : Specification<Assignment>
{
    public AssignmentFilterSpec(
        State? state,
        DateOnly? assignedDate,
        int pageIndex,
        int pageSize,
        string? orderBy,
        bool isDescending,
        string? search,
        Guid? assetId,
        Guid? featuredAssignmentId)
    {

        Query.Where(x => x.State != State.Returned);

        if (state.HasValue) Query.Where(x => x.State == state);

        if (assignedDate.HasValue)
            Query.Where(x => x.AssignedDate == assignedDate || x.Id == featuredAssignmentId);

        if (assetId.HasValue)
            Query.Where(x => x.AssetId == assetId || x.Id == featuredAssignmentId);

        if (!string.IsNullOrWhiteSpace(search))
            Query.Where(x =>
                x.Asset!.AssetCode!.Contains(search) ||
                x.Asset.Name!.Contains(search) ||
                x.Staff!.Users!.First().UserName!.Contains(search) ||
                x.Id == featuredAssignmentId);

        Query
            .ApplyPrimaryOrdering(featuredAssignmentId)
            .ApplySecondaryOrdering(orderBy, isDescending)
            .ApplyPaging(pageIndex, pageSize);
    }

    public AssignmentFilterSpec(
        Guid staffId,
        string? orderBy,
        bool isDescending) =>
        Query.Where(x =>
                x.StaffId == staffId &&
                x.AssignedDate <= DateOnly.FromDateTime(DateTime.Today) &&
                x.State != State.Returned)
            .ApplyPrimaryOrdering(null)
            .ApplySecondaryOrdering(orderBy, isDescending);

    public AssignmentFilterSpec(
        Guid staffId,
        Guid assignmentId)
        => Query.Where(x =>
            x.StaffId == staffId &&
            x.Id == assignmentId &&
            x.State != State.Returned &&
            x.AssignedDate <= DateOnly.FromDateTime(DateTime.Today));
}
