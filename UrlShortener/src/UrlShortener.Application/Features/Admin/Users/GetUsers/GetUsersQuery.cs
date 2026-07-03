using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Common.Models;
using UrlShortener.Persistence.Common.Models;

namespace UrlShortener.Application.Features.Admin.Users.GetUsers;

public sealed record GetUsersQuery : PagedRequest,
    IQuery<PagedResponse<UserResponse>>
{
    public bool? IsActive { get; init; }

    public bool? IsLocked { get; init; }
}