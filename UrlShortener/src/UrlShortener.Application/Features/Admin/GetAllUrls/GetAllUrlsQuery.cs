using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.GetAllUrls;

public sealed record GetAllUrlsQuery()
    : IQuery<IReadOnlyList<AdminUrlResponse>>;