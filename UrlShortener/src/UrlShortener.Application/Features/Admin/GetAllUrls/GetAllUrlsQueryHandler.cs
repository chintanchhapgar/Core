using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Admin.GetAllUrls;

public sealed class GetAllUrlsQueryHandler
    : IQueryHandler<GetAllUrlsQuery, IReadOnlyList<AdminUrlResponse>>
{
    private readonly IShortUrlRepository _repository;

    public GetAllUrlsQueryHandler(
        IShortUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<AdminUrlResponse>> Handle(
        GetAllUrlsQuery request,
        CancellationToken cancellationToken)
    {
        var urls = await _repository.GetAllAsync(cancellationToken);

        return urls.Select(x => new AdminUrlResponse
        {
            Id = x.Id,
            OriginalUrl = x.OriginalUrl,
            ShortCode = x.ShortCode,
            IsActive = x.IsActive,
            ClickCount = x.ClickCount,
            CreatedOnUtc = x.CreatedOnUtc,
            ExpiresOnUtc = x.ExpiresOnUtc
        }).ToList();
    }
}