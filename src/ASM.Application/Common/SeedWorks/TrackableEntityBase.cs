namespace ASM.Application.Common.SeedWorks;

public abstract class TrackableEntityBase : EntityBase
{
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
}
