using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.Users.GetUser;

public sealed record GetUserQuery(Guid UserId)
    : IQuery<UserDetailResponse>;