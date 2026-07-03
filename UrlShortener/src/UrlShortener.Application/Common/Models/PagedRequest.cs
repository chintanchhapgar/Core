namespace UrlShortener.Application.Common.Models;

public abstract record PagedRequest
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 10;

    public string? Search { get; init; }

    public string? SortBy { get; init; }

    public bool Descending { get; init; }
}