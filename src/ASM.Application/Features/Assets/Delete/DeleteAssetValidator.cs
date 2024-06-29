using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using FluentValidation;

namespace ASM.Application.Features.Assets.Delete;

public sealed class DeleteAssetValidator : AbstractValidator<DeleteAssetCommand>
{
    public DeleteAssetValidator(AssetValidator assetValidator)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .SetValidator(assetValidator);
    }
}

public sealed class AssetValidator : AbstractValidator<Guid>
{
    private readonly IReadRepository<Asset> _readRepository;

    public AssetValidator(IReadRepository<Asset> readRepository)
    {
        _readRepository = readRepository;

        RuleFor(id => id)
            .MustAsync(IsNotAssignedState)
            .WithMessage("Asset must not be in assigned state")
            .MustAsync(IsNotHistorical)
            .WithMessage("Asset must not belong to one or more historical assignments");
    }

    private async Task<bool> IsNotAssignedState(Guid id, CancellationToken cancellationToken)
    {
        var asset = await _readRepository.GetByIdAsync(id, cancellationToken);
        Guard.Against.NotFound(id, asset);
        return asset.State != State.Assigned;
    }

    private async Task<bool> IsNotHistorical(Guid id, CancellationToken cancellationToken)
    {
        var asset = await _readRepository.GetByIdAsync(id, cancellationToken);
        Guard.Against.NotFound(id, asset);
        return !asset.Assignments!.Any();
    }
}
