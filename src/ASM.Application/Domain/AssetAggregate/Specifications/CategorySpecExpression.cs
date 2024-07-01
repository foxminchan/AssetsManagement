using Ardalis.Specification;

namespace ASM.Application.Domain.AssetAggregate.Specifications;

public static class CategorySpecExpression
{
    public static ISpecificationBuilder<Category> ApplyOrdering(this ISpecificationBuilder<Category> builder,
        string? orderBy, bool isDescending) =>
        orderBy switch
        {
            nameof(Category.Prefix) => isDescending
                ? builder.OrderByDescending(x => x.Prefix)
                : builder.OrderBy(x => x.Prefix),
            _ => isDescending
                ? builder.OrderByDescending(x => x.Name)
                : builder.OrderBy(x => x.Name)
        };
}
