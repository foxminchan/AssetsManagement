using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ASM.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IServiceProvider serviceProvider,
    ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        const string behavior = nameof(ValidationBehavior<TRequest, TResponse>);

        logger.LogInformation(
            "[{Behavior}] handle request={RequestData} and response={ResponseData}",
            behavior, typeof(TRequest).FullName, typeof(TResponse).FullName);

        logger.LogDebug(
            "[{Behavior}] handle request={Request} with content={RequestData}",
            behavior, typeof(TRequest).FullName, JsonSerializer.Serialize(request));

        var validators = serviceProvider
                             .GetService<IEnumerable<IValidator<TRequest>>>()?.ToList()
                         ?? throw new InvalidOperationException();

        if (validators.Count != 0)
            await Task.WhenAll(
                validators.Select(v => v.HandleValidationAsync(request))
            );

        var response = await next();

        logger.LogInformation(
            "[{Behavior}] handled response={Response} with content={ResponseData}",
            behavior, typeof(TResponse).FullName, JsonSerializer.Serialize(response,
                new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles }));

        return response;
    }
}
