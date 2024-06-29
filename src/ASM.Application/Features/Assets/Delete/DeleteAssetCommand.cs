using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;

namespace ASM.Application.Features.Assets.Delete;

public sealed record DeleteAssetCommand(Guid Id) : ICommand<Result>;

public sealed class DeleteAssetHandler(IRepository<Asset> repository) : ICommandHandler<DeleteAssetCommand, Result>
{
    public async Task<Result> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, asset);

        await repository.DeleteAsync(asset, cancellationToken);

        return Result.Success();
    }
}
