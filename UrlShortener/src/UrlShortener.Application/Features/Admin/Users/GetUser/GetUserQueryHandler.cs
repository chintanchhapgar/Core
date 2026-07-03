using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Admin.Users.GetUser;

public sealed class GetUserQueryHandler
    : IQueryHandler<GetUserQuery, UserDetailResponse>
{
    private readonly IUserRepository _users;

    public GetUserQueryHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<UserDetailResponse> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _users.GetByIdAsync(
            request.UserId,
            cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        var urls = await _users.GetUrlsAsync(
            user.Id,
            cancellationToken);

        return new UserDetailResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UrlCount = urls.Count,
            Urls = urls.Select(x => new UserUrlResponse
            {
                Id = x.Id,
                OriginalUrl = x.OriginalUrl,
                ShortCode = x.ShortCode,
                ClickCount = x.ClickCount,
                IsActive = x.IsActive,
                CreatedOnUtc = x.CreatedOnUtc,
                ExpiresOnUtc = x.ExpiresOnUtc
            }).ToList()
        };
    }
}