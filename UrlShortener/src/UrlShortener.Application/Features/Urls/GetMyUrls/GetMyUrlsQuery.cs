using MediatR;

namespace UrlShortener.Application.Features.Urls.GetMyUrls;

public sealed record GetMyUrlsQuery()
    : IRequest<IReadOnlyList<MyUrlResponse>>;