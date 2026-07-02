using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.Queries.GetAllUrls;

public sealed record GetAllUrlsQuery()
    : IQuery<IReadOnlyList<AdminUrlResponse>>;