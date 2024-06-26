using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;

namespace ASM.Application.Features.Categories.Create;

public sealed record CreateCategoryCommand(
    string Name,
    string Prefix) : ICommand<Result<Guid>>;

public sealed class CreateCategoryHandler(IRepository<Category> repository)
    : ICommandHandler<CreateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = new(request.Name, request.Prefix);

        var result = await repository.AddAsync(category, cancellationToken);

        return result.Id;
    }
}
