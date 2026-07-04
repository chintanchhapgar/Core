using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
using UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;
using UrlShortener.Application.Features.Urls.Queries.Analytics;
using UrlShortener.IntegrationTests.Authentication;
using UrlShortener.IntegrationTests.Common;

namespace UrlShortener.IntegrationTests.Urls;

public sealed class AnalyticsTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AnalyticsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;

        _client = factory.CreateClient(new()
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Analytics_should_return_click_count()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var create = await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://google.com",
                null,
                null));

        var created = await create.Content
            .ReadFromJsonAsync<CreateShortUrlResponse>();

        await _client.GetAsync($"/{created!.ShortCode}");
        await _client.GetAsync($"/{created.ShortCode}");

        // Act
        var response = await _client.GetAsync(
            $"/api/urls/{created.Id}/analytics");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var analytics = await response.Content
            .ReadFromJsonAsync<UrlAnalyticsResponse>();

        analytics.Should().NotBeNull();
        analytics!.TotalClicks.Should().Be(2);
    }

    [Fact]
    public async Task Analytics_should_return_recent_visits()
    {
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var create = await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://github.com",
                null,
                null));

        var created = await create.Content
            .ReadFromJsonAsync<CreateShortUrlResponse>();

        await _client.GetAsync($"/{created!.ShortCode}");

        var response = await _client.GetAsync(
            $"/api/urls/{created.Id}/analytics");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var analytics = await response.Content
            .ReadFromJsonAsync<UrlAnalyticsResponse>();

        analytics.Should().NotBeNull();

        analytics!.RecentVisits.Should().HaveCount(1);
    }

    [Fact]
    public async Task Analytics_should_return_browser_statistics()
    {
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var create = await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://github.com",
                null,
                null));

        var created = await create.Content
            .ReadFromJsonAsync<CreateShortUrlResponse>();

        await _client.GetAsync($"/{created!.ShortCode}");

        var analytics = await _client.GetFromJsonAsync<
            UrlAnalyticsResponse>(
            $"/api/urls/{created.Id}/analytics");

        analytics.Should().NotBeNull();

        analytics!.Browsers.Should().NotBeNull();
        analytics.OperatingSystems.Should().NotBeNull();
    }

    [Fact]
    public async Task Analytics_unknown_url_should_return_not_found()
    {
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var response = await _client.GetAsync(
            $"/api/urls/{Guid.NewGuid()}/analytics");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}