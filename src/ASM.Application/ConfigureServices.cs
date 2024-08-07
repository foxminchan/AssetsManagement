﻿using Ardalis.GuardClauses;
using ASM.Application.Common.Behaviors;
using ASM.Application.Common.Endpoints;
using ASM.Application.Common.Interfaces;
using ASM.Application.Common.Security;
using ASM.Application.Infrastructure.Excel;
using ASM.Application.Infrastructure.Persistence;
using ASM.Application.Infrastructure.Persistence.Interceptors;
using EntityFramework.Exceptions.SqlServer;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ASM.Application;

public static class ConfigureServices
{
    public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    {
        builder.AddIdentityService();

        builder.AddEndpoints(typeof(AssemblyReference));

        builder.Services.AddValidatorsFromAssemblies([AssemblyReference.Executing]);

        builder.Services.AddHttpContextAccessor()
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(AssemblyReference.Executing);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>),
                    ServiceLifetime.Scoped);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>),
                    ServiceLifetime.Scoped);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TxBehavior<,>),
                    ServiceLifetime.Scoped);
            });

        builder.Services.AddScoped(typeof(IExcelWriter<>), typeof(ExcelWriter<>));

        return builder;
    }

    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        builder.Services.AddSingleton<IDatabaseFactory, DatabaseFactory>();

        builder.Services.AddSingleton<AuditableEntityInterceptor>();
        builder.Services.AddSingleton<TrackableEntityInterceptor>();

        builder.Services.AddDbContextPool<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.UseCompatibilityLevel(160);
                        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                        sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    })
                .UseExceptionProcessor();

            options.AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>());
            options.AddInterceptors(sp.GetRequiredService<TrackableEntityInterceptor>());

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
                options
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
        });

        builder.Services.AddScoped<IDatabaseFacade>(p => p.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<IDomainEventContext>(p => p.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<ApplicationDbContextInitializer>();
        builder.Services.AddScoped(typeof(IReadRepository<>), typeof(Repository<>));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return builder;
    }
}
