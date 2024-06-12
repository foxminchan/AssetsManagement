using ASM.Application.Common.SeedWorks;

namespace ASM.Application.Common.Interfaces;

public interface IDomainEventContext
{
    IEnumerable<EventBase> GetDomainEvents();
}
