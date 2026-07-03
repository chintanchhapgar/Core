namespace UrlShortener.Application.Common.Models;

public sealed class PagedResponse<T>
{
    public IReadOnlyList<T> Items { get; init; }
        = [];

    public PaginationMetadata Pagination { get; init; }
        = default!;
}