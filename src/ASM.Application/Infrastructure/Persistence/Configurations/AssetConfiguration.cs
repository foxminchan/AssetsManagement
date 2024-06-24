using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.Shared;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASM.Application.Infrastructure.Persistence.Configurations;

public sealed class AssetConfiguration : BaseConfiguration<Asset>
{
    public override void Configure(EntityTypeBuilder<Asset> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(DataSchemaLength.SuperLarge);

        builder.Property(e => e.AssetCode)
            .IsRequired()
            .HasMaxLength(DataSchemaLength.TinySmall)
            .IsFixedLength();

        builder.HasIndex(e => e.AssetCode)
            .IsUnique();

        builder.Property(e => e.Specification)
            .HasMaxLength(DataSchemaLength.Max);

        builder.Property(e => e.InstallDate)
            .IsRequired();

        builder.Property(e => e.Location)
            .IsRequired();

        builder.Property(e => e.State)
            .IsRequired();

        builder.HasMany(x => x.Assignments)
            .WithOne(x => x.Asset)
            .HasForeignKey(x => x.AssetId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Navigation(x => x.Category)
            .AutoInclude();

        builder.HasData(GetSampleData());
    }

    private static IEnumerable<Asset> GetSampleData()
    {
        var faker = new Faker();

        var categories = new[]
        {
            new { Prefix = "PC", Id = Guid.Parse("039c5946-0dc0-4584-9494-8e00213cbff8") },
            new { Prefix = "LT", Id = Guid.Parse("266bd6bc-9231-44a9-b5c2-af567ac3df10") },
            new { Prefix = "PR", Id = Guid.Parse("5047d5be-aeee-4072-ae47-b860ce5e0ae5") },
            new { Prefix = "BM", Id = Guid.Parse("c0fadf90-721e-4b55-ac30-3567b63c8b8e") },
            new { Prefix = "BS", Id = Guid.Parse("d1b6e7dd-e852-4c62-bee1-92c107d78bd6") },
            new { Prefix = "MN", Id = Guid.Parse("6a5adb7b-94ee-498d-a20f-ec6e3d446df2") },
            new { Prefix = "KB", Id = Guid.Parse("c568c761-8916-4355-9991-247051cf7ea1") },
            new { Prefix = "HP", Id = Guid.Parse("ef849fb7-10da-42aa-9c57-b07f8f33ee14") },
            new { Prefix = "MC", Id = Guid.Parse("a0c9e8a8-4321-4f74-9c9d-a6a881309dcd") },
            new { Prefix = "WC", Id = Guid.Parse("7e9e0aa3-f1d1-46e8-8cf3-27fbeb85ed9c") }
        };

        return categories.SelectMany(category =>
            Enumerable.Range(0, 30).Select(i => new Asset
            {
                Name = faker.Commerce.ProductName(),
                AssetCode = $"{category.Prefix}{i:D6}",
                Specification = faker.Commerce.ProductDescription(),
                InstallDate = faker.Date.FutureDateOnly(),
                Location = faker.PickRandom<Location>(),
                State = faker.PickRandom<State>(),
                CategoryId = category.Id
            })
        );
    }
}
