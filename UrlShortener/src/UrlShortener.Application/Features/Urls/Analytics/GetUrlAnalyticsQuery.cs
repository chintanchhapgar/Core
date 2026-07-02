using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.Analytics;

public sealed record GetUrlAnalyticsQuery(Guid UrlId)
    : IQuery<UrlAnalyticsResponse>;