using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Caching;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.Common.Caching;
using UrlShortener.Application.Common.Extensions;

namespace UrlShortener.Application.Features.Urls.Commands.ActivateUrl;

public sealed class ActivateUrlCommandHandler
    : ICommandHandler<ActivateUrlCommand>
{
    private readonly IShortUrlRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public ActivateUrlCommandHandler(
        IShortUrlRepository repository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _repository = repository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task Handle(
        ActivateUrlCommand request,
        CancellationToken cancellationToken)
    {
        var url = await _repository.GetRequiredAccessibleUrlAsync(
            request.UrlId,
            _currentUser.IsAdmin,
            _currentUser.UserId,
            cancellationToken);

        url.Activate();

        _repository.Update(url);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await CacheInvalidation.InvalidateUrlAsync(
            _cache,
            url.Id,
            url.ShortCode,
            removeAnalytics: false);
    }
}