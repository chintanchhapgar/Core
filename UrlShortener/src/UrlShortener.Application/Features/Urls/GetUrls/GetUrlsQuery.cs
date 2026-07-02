using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.GetUrls;

public sealed record GetUrlsQuery()
    : IQuery<IReadOnlyList<UrlResponse>>;