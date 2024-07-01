using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Specifications;

namespace ASM.Application.Features.Categories.List;

public sealed record ListCategoriesQuery : IQuery<IEnumerable<Category>>;

public sealed class ListCategoriesHandler(IReadRepository<Category> repository) : IQueryHandler<ListCategoriesQuery, IEnumerable<Category>>
{
    public async Task<IEnumerable<Category>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        var spec = new CategoryFilterSpec("Name", false);
        return await repository.ListAsync(spec, cancellationToken);
    }
}
