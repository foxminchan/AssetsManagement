using System.Security.Claims;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace ASM.Api;

public static class IdentityEndpoint
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("/api")
            .MapIdentityApi<ApplicationUser>()
            .WithTags(nameof(ApplicationUser));

        app.MapGet("/api/me", async (UserManager<ApplicationUser> userManager, ClaimsPrincipal user) =>
            {
                var applicationUser = await userManager.GetUserAsync(user);

                if (applicationUser is null)
                    return Results.NotFound();

                var claims = await userManager.GetClaimsAsync(applicationUser);

                return TypedResults.Ok(new AuthUser
                {
                    Id = applicationUser.Id,
                    AccountStatus = applicationUser.AccountStatus,
                    Claims = claims
                });
            })
            .Produces<Ok<AuthUser>>()
            .WithTags(nameof(ApplicationUser))
            .RequireAuthorization();
    }
}

internal sealed class AuthUser
{
    public string Id { get; set; } = string.Empty;
    public AccountStatus? AccountStatus { get; set; }
    public IEnumerable<Claim> Claims { get; set; } = [];
}
