using MediatR;
using UrlShortener.Domain.Interfaces;

namespace UrlShortener.Application.Features.Urls.Queries.Redirect;

public sealed class GetOriginalUrlQueryHandler
    : IRequestHandler<GetOriginalUrlQuery, string?>
{
    private readonly IShortUrlRepository _repository;

    public GetOriginalUrlQueryHandler(IShortUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<string?> Handle(
        GetOriginalUrlQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByShortCodeAsync(
            request.ShortCode,
            cancellationToken);

        if (entity is null)
            return null;

        entity.RegisterClick();

        await _repository.UpdateAsync(entity, cancellationToken);

        return entity.OriginalUrl;
    }
}