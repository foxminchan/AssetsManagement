namespace ASM.Application.Common.SeedWorks;

public abstract class EntityBase : HasDomainEventsBase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDate { get; set; }
    public Guid Version { get; set; } = Guid.NewGuid();
}
