using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;
using UrlShortener.Persistence.Common.Models;

namespace UrlShortener.Application.Features.Urls.GetUrls;

public sealed record GetUrlsQuery : PagedRequest,
    IQuery<PagedResponse<UrlResponse>>
{
    public bool? IsActive { get; init; }
}