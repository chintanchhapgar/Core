namespace UrlShortener.Application.Common.Models;

public sealed class PaginationMetadata
{
    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int TotalRecords { get; init; }

    public int TotalPages { get; init; }

    public bool HasPrevious => PageNumber > 1;

    public bool HasNext => PageNumber < TotalPages;
}