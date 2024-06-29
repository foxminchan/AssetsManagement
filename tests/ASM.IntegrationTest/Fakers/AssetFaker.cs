using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.Shared;
using Bogus;

namespace ASM.IntegrationTest.Fakers;

public sealed class AssetFaker : Faker<Asset>
{
    public AssetFaker()
    {
        RuleFor(x => x.Name, f => f.Commerce.ProductName());
        RuleFor(x => x.Specification, f => f.Commerce.ProductDescription());
        RuleFor(x => x.AssetCode, f => f.Random.AlphaNumeric(8));
        RuleFor(x => x.InstallDate, f => f.Date.FutureDateOnly());
        RuleFor(x => x.Location, f => f.PickRandom<Location>());
        RuleFor(x => x.State, f => f.PickRandom<State>());
        RuleFor(x => x.Category, f => new()
        {
            Id = f.Random.Guid(),
            Prefix = f.Random.String2(2),
            Name = f.Commerce.Categories(1)[0]
        });
    }
}
