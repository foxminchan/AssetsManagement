using ASM.IntegrationTest.Extensions;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace ASM.IntegrationTest.Fixtures;

public sealed class ApplicationFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class
{
    private readonly List<IContainer> _containers = [];
    public WebApplicationFactory<TProgram> Instance { get; private set; } = default!;

    public Task InitializeAsync()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "QC";
        Instance = WithWebHostBuilder(builder => builder.UseEnvironment(env));
        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        return Task
            .WhenAll(_containers.Select(container => container.DisposeAsync().AsTask()))
            .ContinueWith(async _ => await base.DisposeAsync());
    }


    public ApplicationFactory<TProgram> WithDbContainer()
    {
        _containers.Add(new MsSqlBuilder()
            .WithPassword("NashTech@2024")
            .WithCleanUp(true)
            .Build());

        return this;
    }

    public async Task StartContainersAsync(CancellationToken cancellationToken = default)
    {
        if (_containers.Count == 0) return;

        await Task.WhenAll(_containers.Select(container =>
            container.StartWithWaitAndRetryAsync(cancellationToken: cancellationToken)));

        var dbContainer = _containers.OfType<MsSqlContainer>().FirstOrDefault();

        if (dbContainer is not null)
        {
            Instance = Instance.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("ConnectionStrings:DefaultConnection", dbContainer.GetConnectionString());
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IStartupFilter>(new AutoAuthorizeStartupFilter());
                });
            });
        }
    }

    public new HttpClient CreateClient() => Instance.CreateClient();

    public async Task StopContainersAsync()
    {
        if (_containers.Count == 0) return;

        await Task.WhenAll(_containers.Select(container => container.DisposeAsync().AsTask()))
            .ContinueWith(async _ => await base.DisposeAsync())
            .ContinueWith(async _ => await InitializeAsync())
            .ConfigureAwait(false);

        _containers.Clear();
    }

    private class AutoAuthorizeStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                builder.UseMiddleware<AutoAuthorizeMiddleware>();
                next(builder);
            };
        }
    }
}
