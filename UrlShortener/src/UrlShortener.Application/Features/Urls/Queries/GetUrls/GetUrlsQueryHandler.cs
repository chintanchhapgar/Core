using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.Features.Urls.GetUrls;
using UrlShortener.Persistence.Common.Models;

namespace UrlShortener.Application.Features.Urls.Queries.GetUrls;

public sealed class GetUrlsQueryHandler
	: IQueryHandler<GetUrlsQuery, PagedResponse<UrlResponse>>
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