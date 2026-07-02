
using MediatR;
using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Urls.GetMyUrls;

public sealed class GetMyUrlsQueryHandler
    : IRequestHandler<GetMyUrlsQuery, IReadOnlyList<MyUrlResponse>>
{
    private readonly IShortUrlRepository _repository;
    private readonly ICurrentUserService _userContext;

    public GetMyUrlsQueryHandler(
        IShortUrlRepository repository,
        ICurrentUserService userContext
       )
    {
        _repository = repository;
        _userContext = userContext;
    }

    public async Task<IReadOnlyList<MyUrlResponse>> Handle(
        GetMyUrlsQuery request,
        CancellationToken cancellationToken)
    {
        var urls = await _repository.GetUrlsByUserIdAsync(
            _userContext.UserId,
            cancellationToken);

        return urls.Select(x => new MyUrlResponse
        {
            Id = x.Id,
            OriginalUrl = x.OriginalUrl,
            ShortCode = x.ShortCode,
            ClickCount = x.ClickCount,
            CreatedOnUtc = x.CreatedOnUtc
        }).ToList();
    }
}