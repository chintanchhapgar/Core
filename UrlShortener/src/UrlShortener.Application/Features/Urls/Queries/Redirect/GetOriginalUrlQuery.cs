using MediatR;

namespace UrlShortener.Application.Features.Urls.Queries.Redirect;

public sealed record GetOriginalUrlQuery(string ShortCode)
    : IRequest<string?>;
