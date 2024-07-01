using Ardalis.Specification;

namespace ASM.Application.Domain.AssetAggregate.Specifications;

public sealed class CategoryFilterSpec : Specification<Category>
{
    public CategoryFilterSpec(string? orderBy, bool isDescending)
        => Query.ApplyOrdering(orderBy, isDescending);
}
