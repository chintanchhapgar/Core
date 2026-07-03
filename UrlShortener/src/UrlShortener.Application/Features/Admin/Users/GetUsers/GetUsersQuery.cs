using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.Users.GetUsers;

public sealed record GetUsersQuery()
    : IQuery<IReadOnlyList<UserResponse>>;