using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;

namespace ASM.Application.Domain.IdentityAggregate.Events;

public sealed class PasswordUpdatedEvent(ApplicationUser user) : EventBase
{
    public ApplicationUser User { get; set; } = Guard.Against.Null(user);
}
