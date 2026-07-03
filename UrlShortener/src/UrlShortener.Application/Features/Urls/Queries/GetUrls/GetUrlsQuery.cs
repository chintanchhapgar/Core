using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;

namespace UrlShortener.Application.Features.Urls.GetUrls;

public sealed record GetUrlsQuery : PagedRequest,
    IQuery<PagedResponse<UrlResponse>>
{
    public bool? IsActive { get; init; }
}