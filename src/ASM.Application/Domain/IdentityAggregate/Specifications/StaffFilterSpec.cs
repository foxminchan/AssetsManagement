using Ardalis.Specification;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.Shared;

namespace ASM.Application.Domain.IdentityAggregate.Specifications;

public sealed class StaffFilterSpec : Specification<Staff>
{
    public StaffFilterSpec(
        Location location,
        RoleType? roleType,
        int pageIndex,
        int pageSize,
        string? orderBy,
        bool isDescending,
        string? search,
        Guid? featuredStaffId)
    {
        Query.Where(x => !x.IsDeleted && x.Location == location);

        if (roleType.HasValue)
            Query.Where(x => x.RoleType == roleType || x.Id == featuredStaffId);

        if (!string.IsNullOrWhiteSpace(search))
            Query.Where(x =>
                x.StaffCode!.Contains(search) ||
                (x.FirstName + " " + x.LastName).Contains(search) ||
                x.Id == featuredStaffId);

        Query
            .ApplyPrimaryOrdering(featuredStaffId)
            .ApplySecondaryOrdering(orderBy, isDescending)
            .ApplyPaging(pageIndex, pageSize);
    }

    public StaffFilterSpec(Guid id, Location location)
        => Query.Where(x => x.Id == id && x.Location == location && !x.IsDeleted);

    public StaffFilterSpec(IEnumerable<Guid> ids)
        => Query.Where(x => ids.Contains(x.Id));
}
