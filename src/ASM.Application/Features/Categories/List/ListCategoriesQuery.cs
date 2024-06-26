using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;

namespace ASM.Application.Features.Categories.List;

public sealed record ListCategoriesQuery : IQuery<IEnumerable<Category>>;

public sealed class ListCategorieHandler(IReadRepository<Category> repository)
    : IQueryHandler<ListCategoriesQuery, IEnumerable<Category>>
{
    public async Task<IEnumerable<Category>> Handle(ListCategoriesQuery request,
        CancellationToken cancellationToken) => await repository.ListAsync(cancellationToken);
}
