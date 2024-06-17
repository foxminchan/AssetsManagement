using ASM.Application.Common.Constants;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASM.Application.Common.Security;

public static class IdentityExtension
{
    public static IHostApplicationBuilder AddIdentityService(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(AuthRole.Admin, policy =>
                policy.RequireAuthenticatedUser()
                    .RequireRole(AuthRole.Admin))
            .AddPolicy(AuthRole.User, policy =>
                policy.RequireAuthenticatedUser());

        builder.Services
            .AddIdentityApiEndpoints<ApplicationUser>(options => options.Password.RequireUppercase = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return builder;
    }
}
