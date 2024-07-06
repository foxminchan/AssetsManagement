using Ardalis.Specification;

namespace ASM.Application.Domain.ReturningRequestAggregate.Specifications;

public static class ReturningRequestSpecExpression
{
    public static ISpecificationBuilder<ReturningRequest> ApplyOrdering(
        this ISpecificationBuilder<ReturningRequest> builder,
        string? orderBy, bool isDescending) =>
        orderBy switch
        {
            nameof(ReturningRequest.Assignment.Asset.AssetCode) => isDescending
                ? builder.OrderByDescending(x => x.Assignment!.Asset!.AssetCode)
                : builder.OrderBy(x => x.Assignment!.Asset!.AssetCode),
            nameof(ReturningRequest.Assignment.Asset.Name) => isDescending
                ? builder.OrderByDescending(x => x.Assignment!.Asset!.Name)
                : builder.OrderBy(x => x.Assignment!.Asset!.Name),
            nameof(ReturningRequest.RequestedBy) => isDescending
                ? builder.OrderByDescending(x => x.RequestedBy)
                : builder.OrderBy(x => x.RequestedBy),
            nameof(ReturningRequest.Assignment.AssignedDate) => isDescending
                ? builder.OrderByDescending(x => x.Assignment!.AssignedDate)
                : builder.OrderBy(x => x.Assignment!.AssignedDate),
            nameof(ReturningRequest.AcceptBy) => isDescending
                ? builder.OrderByDescending(x => x.AcceptBy)
                : builder.OrderBy(x => x.AcceptBy),
            nameof(ReturningRequest.ReturnedDate) => isDescending
                ? builder.OrderByDescending(x => x.ReturnedDate)
                : builder.OrderBy(x => x.ReturnedDate),
            nameof(ReturningRequest.State) => isDescending
                ? builder.OrderByDescending(x => x.State)
                : builder.OrderBy(x => x.State),
            _ => isDescending
                ? builder.OrderByDescending(x => x.Assignment!.Asset!.AssetCode)
                : builder.OrderBy(x => x.Assignment!.Asset!.AssetCode)
        };

    public static ISpecificationBuilder<ReturningRequest> ApplyPaging(
        this ISpecificationBuilder<ReturningRequest> builder,
        int pageIndex, int pageSize) =>
        builder
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
}
