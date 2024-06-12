using ASM.Application.Common.SeedWorks;
using Microsoft.AspNetCore.Identity;

namespace ASM.Application.Domain.IdentityAggregate;

public sealed class ApplicationUser : IdentityUser, IAggregateRoot
{
    
}
