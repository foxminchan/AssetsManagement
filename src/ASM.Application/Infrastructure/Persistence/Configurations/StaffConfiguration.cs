using ASM.Application.Domain.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASM.Application.Infrastructure.Persistence.Configurations;

public sealed class StaffConfiguration : BaseConfiguration<Staff>
{
    public override void Configure(EntityTypeBuilder<Staff> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.FirstName)
            .HasMaxLength(DataSchemaLength.Small)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(DataSchemaLength.Medium)
            .IsRequired();

        builder.Property(x => x.StaffCode)
            .HasMaxLength(DataSchemaLength.Micro)
            .IsFixedLength()
            .IsRequired();

        builder.HasIndex(x => x.StaffCode)
            .IsUnique();

        builder.Property(x => x.Dob)
            .IsRequired();

        builder.Property(x => x.JoinedDate)
            .IsRequired();

        builder.Property(x => x.Gender)
            .IsRequired();

        builder.Property(x => x.RoleType)
            .IsRequired();

        builder.Property(x => x.Location)
            .IsRequired();

        builder.Navigation(x => x.Users)
            .AutoInclude();

        builder.HasMany(x => x.Users)
            .WithOne(x => x.Staff)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
