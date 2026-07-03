using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Urls.GetUrls;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;

public sealed class GetUrlsQueryHandler
    : IQueryHandler<GetUrlsQuery, PagedResponse<UrlResponse>>
{
    private readonly IShortUrlRepository _repository;
    private readonly ICurrentUser _currentUser;

    public GetUrlsQueryHandler(
        IShortUrlRepository repository,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<PagedResponse<UrlResponse>> Handle(
        GetUrlsQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetPagedAccessibleUrlsAsync(
            request,
            _currentUser.IsAdmin,
            _currentUser.UserId,
            cancellationToken);
    }
}