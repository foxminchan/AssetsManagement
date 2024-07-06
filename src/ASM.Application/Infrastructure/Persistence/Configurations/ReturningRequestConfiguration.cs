using ASM.Application.Domain.ReturningRequestAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASM.Application.Infrastructure.Persistence.Configurations;

public sealed class ReturningRequestConfiguration : BaseConfiguration<ReturningRequest>
{
    public override void Configure(EntityTypeBuilder<ReturningRequest> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.State)
            .IsRequired();

        builder.Navigation(x => x.Assignment)
            .AutoInclude();

        builder.Navigation(x => x.Staff)
            .AutoInclude();
    }
}
