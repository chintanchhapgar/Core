using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace UrlShortener.API.Extensions;

public static class HealthCheckExtensions
{
    public static IEndpointRouteBuilder MapApplicationHealthChecks(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    status = report.Status.ToString(),
                    totalDuration = report.TotalDuration.TotalMilliseconds,
                    checks = report.Entries.Select(x => new
                    {
                        name = x.Key,
                        status = x.Value.Status.ToString(),
                        description = x.Value.Description,
                        duration = x.Value.Duration.TotalMilliseconds
                    })
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        response,
                        new JsonSerializerOptions
                        {
                            WriteIndented = true
                        }));
            }
        });

        return endpoints;
    }
}