namespace UrlShortener.Application.Features.Admin.Dashboard;

public sealed class DashboardResponse
{
    public int TotalUsers { get; init; }

    public int TotalUrls { get; init; }

    public int ActiveUrls { get; init; }

    public int ExpiredUrls { get; init; }

    public int TotalClicks { get; init; }
}