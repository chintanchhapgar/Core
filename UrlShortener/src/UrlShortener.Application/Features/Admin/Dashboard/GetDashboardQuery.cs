using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.Dashboard;

public sealed record GetDashboardQuery()
    : IQuery<DashboardResponse>;