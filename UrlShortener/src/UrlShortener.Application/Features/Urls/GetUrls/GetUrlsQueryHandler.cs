using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.GetUrls;

public sealed class GetUrlsQueryHandler
    : IQueryHandler<GetUrlsQuery, IReadOnlyList<UrlResponse>>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IShortUrlRepository _repository;

    public GetUrlsQueryHandler(
        ICurrentUserService currentUser,
        IShortUrlRepository repository)
    {
        _currentUser = currentUser;
        _repository = repository;
    }

    public async Task<IReadOnlyList<UrlResponse>> Handle(
        GetUrlsQuery request,
        CancellationToken cancellationToken)
    {
        IReadOnlyList<ShortUrl> urls;

        if (_currentUser.IsAdmin)
        {
            urls = await _repository.GetAllAsync(cancellationToken);
        }
        else
        {
            urls = await _repository.GetUrlsByUserIdAsync(
                _currentUser.UserId,
                cancellationToken);
        }

        return urls.Select(x => new UrlResponse
        {
            Id = x.Id,
            OriginalUrl = x.OriginalUrl,
            ShortCode = x.ShortCode,
            ClickCount = x.ClickCount,
            IsActive = x.IsActive,
            CreatedOnUtc = x.CreatedOnUtc,
            ExpiresOnUtc = x.ExpiresOnUtc
        }).ToList();
    }
}