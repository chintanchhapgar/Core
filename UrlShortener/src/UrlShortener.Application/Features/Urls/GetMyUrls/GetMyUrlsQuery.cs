using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.GetMyUrl;

public sealed record GetMyUrlQuery(Guid UrlId)
    : IQuery<MyUrlDetailResponse>;