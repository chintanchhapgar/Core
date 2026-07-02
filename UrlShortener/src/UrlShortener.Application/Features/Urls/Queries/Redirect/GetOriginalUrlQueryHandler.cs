using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Urls.Commands.ResolveShortUrl;

public sealed class ResolveShortUrlCommandHandler
    : ICommandHandler<ResolveShortUrlCommand, string?>
{
    private readonly IShortUrlRepository _repository;

    public ResolveShortUrlCommandHandler(
        IShortUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<string?> Handle(
        ResolveShortUrlCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByShortCodeAsync(
            request.ShortCode,
            cancellationToken);

        if (entity is null)
            return null;

        // Prevent redirect if disabled
        if (!entity.IsActive)
        {
            throw new InvalidOperationException(
                "This short URL has been deactivated.");
        }

        // Prevent redirect if expired
        if (entity.IsExpired())
        {
            throw new InvalidOperationException(
                "This short URL has expired.");
        }

        entity.RegisterClick();

        _repository.Update(entity);

        return entity.OriginalUrl;
    }
}