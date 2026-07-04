using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UrlShortener.API.HealthChecks;

public sealed class MemoryHealthCheck : IHealthCheck
{
    private const long MaxMemory = 512 * 1024 * 1024; // 512 MB

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var memory = GC.GetTotalMemory(false);

        if (memory > MaxMemory)
        {
            return Task.FromResult(
                HealthCheckResult.Degraded(
                    $"Memory usage is high ({memory / 1024 / 1024} MB)."));
        }

        return Task.FromResult(
            HealthCheckResult.Healthy(
                $"Memory usage: {memory / 1024 / 1024} MB."));
    }
}