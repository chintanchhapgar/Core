using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Urls.GetMyUrl;

public sealed class GetMyUrlQueryHandler
    : IQueryHandler<GetMyUrlQuery, MyUrlDetailResponse>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IShortUrlRepository _repository;

    public GetMyUrlQueryHandler(
        ICurrentUserService currentUser,
        IShortUrlRepository repository)
    {
        _currentUser = currentUser;
        _repository = repository;
    }

    public async Task<MyUrlDetailResponse> Handle(
        GetMyUrlQuery request,
        CancellationToken cancellationToken)
    {
        var url = await _repository.GetByIdAndUserAsync(
            request.UrlId,
            _currentUser.UserId!.Value,
            cancellationToken);

        if (url is null)
            throw new KeyNotFoundException("Short URL not found.");

        return new MyUrlDetailResponse
        {
            Id = url.Id,
            OriginalUrl = url.OriginalUrl,
            ShortCode = url.ShortCode,
            IsActive = url.IsActive,
            ClickCount = url.ClickCount,
            CreatedOnUtc = url.CreatedOnUtc,
            ExpiresOnUtc = url.ExpiresOnUtc
        };
    }
}