using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;

namespace ASM.Application.Features.Assets.Update;

public sealed record UpdateAssetCommand(
    Guid Id,
    string? Name,
    string? Specification,
    DateOnly InstalledDate,
    State State) : ICommand<Result>;

public sealed class UpdateAssetHandler(IRepository<Asset> repository) : ICommandHandler<UpdateAssetCommand, Result>
{
    public async Task<Result> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, asset);

        asset.Update(request.Name, request.Specification, request.InstalledDate, request.State);

        await repository.UpdateAsync(asset, cancellationToken);

        return Result.Success();
    }
}
