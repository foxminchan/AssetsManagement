using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.Shared;

namespace ASM.Application.Domain.AssetAggregate;

public sealed class Asset : EntityBase, IAggregateRoot
{
    public Asset()
    {
        // EF Mapping
    }

    public Asset(string? name, string? assetCode, string? specification, DateOnly installDate, State state,
        Location location, Guid categoryId)
    {
        Name = Guard.Against.NullOrEmpty(name);
        AssetCode = Guard.Against.NullOrEmpty(assetCode);
        Specification = Guard.Against.NullOrEmpty(specification);
        InstallDate = Guard.Against.NullOrOutOfRange(installDate, nameof(installDate),
            DateOnly.FromDateTime(DateTime.Now), DateOnly.MaxValue);
        State = Guard.Against.EnumOutOfRange(state);
        Location = Guard.Against.EnumOutOfRange(location);
        CategoryId = categoryId;
    }

    public string? Name { get; set; }
    public string? AssetCode { get; set; }
    public string? Specification { get; set; }
    public DateOnly InstallDate { get; set; }
    public State State { get; set; }
    public Location Location { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<Assignment>? Assignments { get; set; } = [];

    public static string GenerateAssetCode(List<Asset> assets, Guid categoryId)
    {
        var prefix = assets.First(x => x.CategoryId == categoryId).Category?.Prefix;
        var assetCode = $"{prefix}000001";
        var count = 1;

        while (assets.Exists(x => x.AssetCode == assetCode))
        {
            assetCode = $"{prefix}{count:D6}";
            count++;
        }

        return assetCode;
    }

    public void Update(string? name, string? specification, DateOnly installedDate, State state)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Specification = specification;
        InstallDate = Guard.Against.NullOrOutOfRange(installedDate, nameof(installedDate),
            DateOnly.FromDateTime(DateTime.Now), DateOnly.MaxValue);
        State = state;
    }
}
