using FluentValidation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ASM.Application.Common;

public static class Validation
{
    public static async Task HandleValidationAsync<TRequest>(this IValidator<TRequest> validator, TRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);
        var failures = validationResult.Errors;
        if (failures.Count != 0) throw new ValidationException(failures);
    }

    public static IHostApplicationBuilder AddValidator(this IHostApplicationBuilder builder)
    {
        builder.Services.Scan(scan => scan
            .FromAssemblies(AssemblyReference.Assembly)
            .AddClasses(c => c.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return builder;
    }
}
