using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.Queries.Analytics;

public sealed record GetUrlAnalyticsQuery(Guid UrlId)
    : IQuery<UrlAnalyticsResponse>;