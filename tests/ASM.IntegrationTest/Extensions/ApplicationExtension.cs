using ASM.Application.Infrastructure.Persistence;
using ASM.IntegrationTest.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace ASM.IntegrationTest.Extensions;

public static class ApplicationExtension
{
    public static async Task EnsureCreatedAsync<T>(
        this ApplicationFactory<T> factory,
        CancellationToken cancellationToken = default)
        where T : class
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }

    public static async Task EnsureCreatedAndPopulateDataAsync<TProgram, TEntity>(
        this ApplicationFactory<TProgram> factory,
        IReadOnlyCollection<TEntity> entities,
        CancellationToken cancellationToken = default)
        where TProgram : class
        where TEntity : class
    {
        await using var scope = factory.Instance.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
