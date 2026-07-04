namespace UrlShortener.Infrastructure.Configuration;

public sealed class CleanupJobOptions
{
    public const string SectionName = "CleanupJob";

    public int IntervalMinutes { get; init; } = 10;
}