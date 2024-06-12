using ASM.Application.Common.SeedWorks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ASM.Application.Infrastructure.Persistence.Configurations;

public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.CreatedDate)
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.UpdateDate)
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(e => e.Version)
            .IsConcurrencyToken();
    }
}
