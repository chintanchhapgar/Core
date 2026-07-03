using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Caching;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.Common.Caching;
using UrlShortener.Application.Common.Extensions;

namespace UrlShortener.Application.Features.Urls.Commands.UpdateUrl;

public sealed class UpdateUrlCommandHandler
    : ICommandHandler<UpdateUrlCommand>
{
    private readonly ICurrentUser _currentUser;
    private readonly IShortUrlRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public UpdateUrlCommandHandler(
        ICurrentUser currentUser,
        IShortUrlRepository repository,
        IUnitOfWork unitOfWork,
        ICacheService cache)
    {
        _currentUser = currentUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task Handle(
        UpdateUrlCommand request,
        CancellationToken cancellationToken)
    {
        var url = await _repository.GetRequiredAccessibleUrlAsync(
            request.UrlId,
            _currentUser.IsAdmin,
            _currentUser.UserId,
            cancellationToken);

        var oldShortCode = url.ShortCode;

        url.Update(
            request.OriginalUrl,
            request.ExpirationDateUtc);

        _repository.Update(url);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await CacheInvalidation.InvalidateUrlAsync(
            _cache,
            url.Id,
            url.ShortCode);
    }
}