using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;

namespace UrlShortener.Application.Features.Urls.Queries.GetUrls;

public sealed class GetUrlsQueryHandler
    : IQueryHandler<GetUrlsQuery, IReadOnlyList<UrlResponse>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IShortUrlRepository _repository;

    public GetUrlsQueryHandler(
        ICurrentUser currentUser,
        IShortUrlRepository repository)
    {
        _currentUser = currentUser;
        _repository = repository;
    }

    public async Task<IReadOnlyList<UrlResponse>> Handle(
        GetUrlsQuery request,
        CancellationToken cancellationToken)
    {
        var urls = await _repository.GetAccessibleUrlsAsync(
            _currentUser.IsAdmin,
            _currentUser.UserId,
            cancellationToken);

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