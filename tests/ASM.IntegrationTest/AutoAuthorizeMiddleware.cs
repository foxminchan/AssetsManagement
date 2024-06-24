using System.Security.Claims;
using ASM.Application.Common.Constants;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace ASM.IntegrationTest;

public sealed class AutoAuthorizeMiddleware(RequestDelegate rd)
{
    public async Task Invoke(HttpContext httpContext)
    {
        var identity = new ClaimsIdentity("cookies");

        identity.AddClaim(new(nameof(AuthRole), AuthRole.Admin));
        identity.AddClaim(new("Status", nameof(AccountStatus.Active)));
        identity.AddClaim(new(nameof(ApplicationUser.UserName), "nhannx"));
        identity.AddClaim(new(nameof(Location), nameof(Location.HoChiMinh)));

        httpContext.User.AddIdentity(identity);

        await rd.Invoke(httpContext);
    }
}
