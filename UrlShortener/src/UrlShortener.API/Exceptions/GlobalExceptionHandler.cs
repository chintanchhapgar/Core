using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using UrlShortener.Application.Common.Exceptions;

namespace UrlShortener.API.Exceptions;

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
        _logger.LogError(exception, "An unhandled exception occurred.");

        var statusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            DuplicateShortCodeException => StatusCodes.Status409Conflict,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        ProblemDetails problem;

        if (exception is ValidationException validationException)
        {
            problem = new ValidationProblemDetails(
                validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()))
            {
                Title = "Validation Failed",
                Status = statusCode,
                Instance = httpContext.Request.Path
            };
        }
        else
        {
            problem = new ProblemDetails
            {
                Title = ReasonPhrases.GetReasonPhrase(statusCode),
                Detail = exception.Message,
                Status = statusCode,
                Instance = httpContext.Request.Path
            };
        }

        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problem,
            cancellationToken);

        return true;
    }
}