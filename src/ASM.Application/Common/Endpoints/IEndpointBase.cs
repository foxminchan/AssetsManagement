using Microsoft.AspNetCore.Routing;

namespace ASM.Application.Common.Endpoints;

public interface IEndpointBase
{
    void MapEndpoint(IEndpointRouteBuilder app);
}