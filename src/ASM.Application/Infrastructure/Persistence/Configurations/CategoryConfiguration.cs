using ASM.Application.Domain.AssetAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASM.Application.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : BaseConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(DataSchemaLength.Large);

        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.Property(e => e.Prefix)
            .IsRequired()
            .HasMaxLength(DataSchemaLength.Tiny)
            .IsFixedLength();

        builder.HasIndex(e => e.Prefix)
            .IsUnique();

        builder.HasData(GetSampleData());

        builder.HasMany(x => x.Assets)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static IEnumerable<Category> GetSampleData()
    {
        yield return new()
        {
            Id = new("039c5946-0dc0-4584-9494-8e00213cbff8"),
            Name = "Personal Computer",
            Prefix = "PC"
        };

        yield return new()
        {
            Id = new("266bd6bc-9231-44a9-b5c2-af567ac3df10"),
            Name = "Laptop",
            Prefix = "LT"
        };

        yield return new()
        {
            Id = new("5047d5be-aeee-4072-ae47-b860ce5e0ae5"),
            Name = "Printer",
            Prefix = "PR"
        };

        yield return new()
        {
            Id = new("c0fadf90-721e-4b55-ac30-3567b63c8b8e"),
            Name = "Bluetooth Mouse",
            Prefix = "BM"
        };

        yield return new()
        {
            Id = new("d1b6e7dd-e852-4c62-bee1-92c107d78bd6"),
            Name = "Bluetooth Speaker",
            Prefix = "BS"
        };

        yield return new()
        {
            Id = new("6a5adb7b-94ee-498d-a20f-ec6e3d446df2"),
            Name = "Monitor",
            Prefix = "MN"
        };

        yield return new()
        {
            Id = new("c568c761-8916-4355-9991-247051cf7ea1"),
            Name = "Keyboard",
            Prefix = "KB"
        };

        yield return new()
        {
            Id = new("ef849fb7-10da-42aa-9c57-b07f8f33ee14"),
            Name = "Headphone",
            Prefix = "HP"
        };

        yield return new()
        {
            Id = new("a0c9e8a8-4321-4f74-9c9d-a6a881309dcd"),
            Name = "Microphone",
            Prefix = "MC"
        };

        yield return new()
        {
            Id = new("7e9e0aa3-f1d1-46e8-8cf3-27fbeb85ed9c"),
            Name = "Webcam",
            Prefix = "WC"
        };
    }
}
