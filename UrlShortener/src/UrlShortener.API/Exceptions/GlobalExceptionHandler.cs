using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using UrlShortener.Application.Common.Exceptions;

namespace UrlShortener.Api.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "An unhandled exception occurred.");

        var statusCode = exception switch
        {
            DuplicateShortCodeException => StatusCodes.Status409Conflict,

            KeyNotFoundException => StatusCodes.Status404NotFound,

            //ValidationException => StatusCodes.Status400BadRequest,

            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

            _ => StatusCodes.Status500InternalServerError
        };

        var problem = new ProblemDetails
        {
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Detail = exception.Message,
            Status = statusCode,
            Instance = httpContext.Request.Path
        };

        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problem,
            cancellationToken);

        return true;
    }
}