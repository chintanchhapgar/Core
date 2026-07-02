using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.Queries.GetUrls;

public sealed record GetUrlsQuery()
    : IQuery<IReadOnlyList<UrlResponse>>;