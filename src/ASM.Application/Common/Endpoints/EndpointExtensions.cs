using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASM.Application.Common.Endpoints;

public static class EndpointExtensions
{
    public static void AddEndpoints(this IHostApplicationBuilder builder, Type type) =>
        builder.Services.Scan(scan => scan
            .FromAssembliesOf(type)
            .AddClasses(classes => classes.AssignableTo<IEndpointBase>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var scope = app.Services.CreateScope();

        var endpoints = scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpointBase>>();

        IEndpointRouteBuilder builder = app.MapGroup("/api");

        foreach (var endpoint in endpoints) endpoint.MapEndpoint(builder);

        return app;
    }
}
