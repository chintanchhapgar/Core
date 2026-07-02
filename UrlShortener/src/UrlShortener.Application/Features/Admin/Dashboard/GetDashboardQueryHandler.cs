using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Admin.Dashboard;

public sealed class GetDashboardQueryHandler
    : IQueryHandler<GetDashboardQuery, DashboardResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IShortUrlRepository _shortUrlRepository;

    public GetDashboardQueryHandler(
        IUserRepository userRepository,
        IShortUrlRepository shortUrlRepository)
    {
        _userRepository = userRepository;
        _shortUrlRepository = shortUrlRepository;
    }

    public async Task<DashboardResponse> Handle(
        GetDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var totalUsers = await _userRepository.CountAsync(
            cancellationToken);

        var totalUrls = await _shortUrlRepository.CountAsync(
            cancellationToken);

        var activeUrls = await _shortUrlRepository.CountActiveAsync(
            cancellationToken);

        var expiredUrls = await _shortUrlRepository.CountExpiredAsync(
            cancellationToken);

        var totalClicks = await _shortUrlRepository.TotalClicksAsync(
            cancellationToken);

        return new DashboardResponse
        {
            TotalUsers = totalUsers,
            TotalUrls = totalUrls,
            ActiveUrls = activeUrls,
            ExpiredUrls = expiredUrls,
            TotalClicks = totalClicks
        };
    }
}