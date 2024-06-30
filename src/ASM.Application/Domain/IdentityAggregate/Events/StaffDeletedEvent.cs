using Ardalis.GuardClauses;
using ASM.Application.Common.SeedWorks;

namespace ASM.Application.Domain.IdentityAggregate.Events;

public sealed class StaffDeletedEvent(string userId) : EventBase
{
    public string UserId { get; set; } = Guard.Against.NullOrEmpty(userId);
}
