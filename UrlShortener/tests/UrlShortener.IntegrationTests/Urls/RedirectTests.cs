using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;
using UrlShortener.IntegrationTests.Authentication;
using UrlShortener.IntegrationTests.Common;

namespace UrlShortener.IntegrationTests.Urls;

public sealed class RedirectTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public RedirectTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;

        _client = factory.CreateClient(new()
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Redirect_should_return_302()
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

        // Act
        var response = await _client.GetAsync($"/{created!.ShortCode}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!
        .ToString()
        .TrimEnd('/')
        .Should()
        .Be("https://google.com");
    }

    [Fact]
    public async Task Redirect_unknown_shortcode_should_return_not_found()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        // Act
        var response = await _client.GetAsync("/unknown123");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Redirect_should_increment_click_count()
    {
        // Arrange
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

        // Redirect
        await _client.GetAsync($"/{created!.ShortCode}");

        // Read list
        var list = await _client.GetFromJsonAsync<
            PagedResponse<UrlResponse>>("/api/urls");

        list.Should().NotBeNull();

        list!.Items
            .Single(x => x.Id == created.Id)
            .ClickCount
            .Should()
            .Be(1);
    }

    [Fact]
    public async Task Redirect_inactive_url_should_return_not_found()
    {
        // Arrange
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

        await _client.PutAsync(
            $"/api/urls/{created!.Id}/deactivate",
            null);

        // Act
        var response = await _client.GetAsync(
            $"/{created.ShortCode}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Redirect_expired_url_should_return_not_found()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var create = await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://github.com",
                null,
                DateTime.UtcNow.AddMinutes(-10)));

        var created = await create.Content
            .ReadFromJsonAsync<CreateShortUrlResponse>();

        // Act
        var response = await _client.GetAsync(
            $"/{created!.ShortCode}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}