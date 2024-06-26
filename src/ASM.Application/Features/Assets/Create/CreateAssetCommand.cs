using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace ASM.Application.Features.Assets.Create;

public sealed record CreateAssetCommand(
    string? Name,
    string? Specification,
    DateOnly InstallDate,
    State State,
    Guid CategoryId) : ICommand<Result<Guid>>;

public sealed class CreateAssetHandler(IRepository<Asset> repository, IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<CreateAssetCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
    {
        var location = httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => x.Type == nameof(Location))
            ?.Value;

        Guard.Against.NullOrEmpty(location);

        var assets = await repository.ListAsync(cancellationToken);

        var assetCode = Asset.GenerateAssetCode(assets, request.CategoryId);

        Asset asset = new(
            request.Name,
            assetCode,
            request.Specification,
            request.InstallDate,
            request.State,
            Enum.Parse<Location>(location),
            request.CategoryId);

        var result = await repository.AddAsync(asset, cancellationToken);

        return result.Id;
    }
}
