using ASM.Application.Domain.AssignmentAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASM.Application.Infrastructure.Persistence.Configurations;

public sealed class AssignmentConfiguration : BaseConfiguration<Assignment>
{
    public override void Configure(EntityTypeBuilder<Assignment> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.State)
            .IsRequired();

        builder.Property(e => e.AssignedDate)
            .IsRequired();

        builder.Property(e => e.Note)
            .HasMaxLength(DataSchemaLength.Max);

        builder.Navigation(x => x.Asset)
            .AutoInclude();

        builder.Navigation(x => x.Staff)
            .AutoInclude();
    }
}
