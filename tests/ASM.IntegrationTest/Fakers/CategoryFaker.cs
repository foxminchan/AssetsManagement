using ASM.Application.Domain.AssetAggregate;
using Bogus;

namespace ASM.IntegrationTest.Fakers;

public sealed class CategoryFaker : Faker<Category>
{
    public CategoryFaker()
    {
        RuleFor(x => x.Id, f => f.Random.Guid());
        RuleFor(x => x.Prefix, f => f.Random.String2(2));
        RuleFor(x => x.Name, f => f.Commerce.Categories(1)[0]);
    }
}
