using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;

namespace ASM.Application.Domain.AssetAggregate;

public sealed class Category : EntityBase, IAggregateRoot
{
    public Category()
    {
        // EF Mapping
    }

    public Category(string name, string prefix)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Prefix = Guard.Against.NullOrEmpty(prefix);
    }

    public string? Name { get; set; }
    public string? Prefix { get; set; }
    public ICollection<Asset>? Assets { get; set; } = [];
}
