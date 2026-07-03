using UrlShortener.Application.Abstractions.Caching;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Common.Caching;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Commands.ResolveShortUrl;

public sealed class ResolveShortUrlCommandHandler
    : ICommandHandler<ResolveShortUrlCommand, string?>
{
    private readonly IShortUrlRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public ResolveShortUrlCommandHandler(
        IShortUrlRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<string?> Handle(
        ResolveShortUrlCommand request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.Url(request.ShortCode);

        var cached = await _cache.GetAsync<CachedShortUrl>(cacheKey);

        if (cached is null)
        {
            var entity = await _repository.GetByShortCodeAsync(
                request.ShortCode,
                cancellationToken);

            if (entity is null)
                return null;

            if (!entity.IsActive || entity.IsExpired())
                return null;

            cached = new CachedShortUrl
            {
                Id = entity.Id,
                OriginalUrl = entity.OriginalUrl,
                ShortCode = entity.ShortCode,
                IsActive = entity.IsActive,
                ExpiresOnUtc = entity.ExpiresOnUtc
            };

            await _cache.SetAsync(
                cacheKey,
                cached,
                TimeSpan.FromMinutes(30));
        }

        var shortUrl = await _repository.GetByIdAsync(
            cached.Id,
            cancellationToken);

        if (shortUrl is null)
            return null;

        shortUrl.RegisterClick();

        await _repository.AddVisitAsync(
            new ShortUrlVisit(
                shortUrl.Id,
                null,
                null,
                null,
                null,
                null),
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Invalidate caches affected by a new visit
        await CacheInvalidation.InvalidateAnalyticsAsync(
            _cache,
            shortUrl.Id);

        return cached.OriginalUrl;
    }
}