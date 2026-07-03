using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Admin.Users.GetUsers;

public sealed record GetUsersQuery : PagedRequest,
    IQuery<PagedResponse<UserResponse>>;