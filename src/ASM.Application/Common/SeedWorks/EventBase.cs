using MediatR;

namespace ASM.Application.Common.SeedWorks;

public abstract class EventBase : INotification
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
