﻿using Ardalis.GuardClauses;
using Ardalis.Result;
using EntityFramework.Exceptions.Common;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ASM.Api.Middleware;

public sealed class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "[{Handler}] Exception occurred: {ExceptionMessage}", nameof(ExceptionHandler),
            exception.Message);

        switch (exception)
        {
            case ValidationException validationException:
                await HandleValidationException(httpContext, validationException, cancellationToken);
                break;

            case NotFoundException notFoundException:
                await HandleNotFoundException(httpContext, notFoundException, cancellationToken);
                break;

            case UniqueConstraintException uniqueConstraintException:
                await HandleUniqueConstraintException(httpContext, uniqueConstraintException, cancellationToken);
                break;

            default:
                await HandleDefaultException(httpContext, exception, cancellationToken);
                break;
        }

        return true;
    }

    private static async ValueTask<bool> HandleValidationException(
        HttpContext httpContext,
        ValidationException validationException,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var validationErrorModel = Result.Invalid(validationException
            .Errors
            .Select(e => new ValidationError(
                e.PropertyName,
                e.ErrorMessage,
                StatusCodes.Status400BadRequest.ToString(),
                ValidationSeverity.Info
            )).ToList());

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(TypedResults.BadRequest(validationErrorModel.ValidationErrors),
            cancellationToken);

        return true;
    }

    private static async ValueTask<bool> HandleNotFoundException(
        HttpContext httpContext,
        Exception notFoundException,
        CancellationToken cancellationToken)
    {
        var notFoundErrorModel = Result.NotFound(notFoundException.Message);

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(TypedResults.NotFound(notFoundErrorModel.Errors),
            cancellationToken);

        return true;
    }

    private static async ValueTask<bool> HandleUniqueConstraintException(
        HttpContext httpContext,
        UniqueConstraintException uniqueConstraintException,
        CancellationToken cancellationToken)
    {
        var uniqueConstraintErrorModel = Result.Invalid(
            new ValidationError(
                uniqueConstraintException.ConstraintName,
                uniqueConstraintException.Message,
                StatusCodes.Status409Conflict.ToString(),
                ValidationSeverity.Info
            ));

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        await httpContext.Response.WriteAsJsonAsync(TypedResults.Conflict(uniqueConstraintErrorModel.ValidationErrors),
            cancellationToken);

        return true;
    }

    private static async ValueTask<bool> HandleDefaultException(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails details = new()
        {
            Type = exception.GetType().Name,
            Title = "InternalServerError",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(TypedResults.Problem(details), cancellationToken);

        return true;
    }
}
