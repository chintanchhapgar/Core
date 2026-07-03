using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Common.Models;

namespace UrlShortener.Application.Features.Admin.Users.GetUsers;

public sealed class GetUsersQueryHandler
	: IQueryHandler<GetUsersQuery, PagedResponse<UserResponse>>
{
	private readonly IUserRepository _repository;

	public GetUsersQueryHandler(IUserRepository repository)
	{
		_repository = repository;
	}

	public async Task<PagedResponse<UserResponse>> Handle(
		GetUsersQuery request,
		CancellationToken cancellationToken)
	{
		return await _repository.GetPagedAsync(
			request,
			cancellationToken);
	}
}