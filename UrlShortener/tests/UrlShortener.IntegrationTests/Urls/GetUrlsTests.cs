using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;
using UrlShortener.IntegrationTests.Authentication;
using UrlShortener.IntegrationTests.Common;

namespace UrlShortener.IntegrationTests.Urls;

public sealed class GetUrlsTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public GetUrlsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_urls_should_return_empty_collection_for_new_user()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        // Act
        var response = await _client.GetAsync("/api/urls");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<
            PagedResponse<UrlResponse>>();

        result.Should().NotBeNull();
        result!.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Get_urls_should_return_success()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        // Act
        var response = await _client.GetAsync("/api/urls");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Created_url_should_be_returned()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://github.com",
                null,
                null));

        // Act
        var response = await _client.GetAsync("/api/urls");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<
            PagedResponse<UrlResponse>>();

        result.Should().NotBeNull();
        result!.Items.Should().ContainSingle();
        result.Items[0].OriginalUrl.Should().Be("https://github.com");
    }

    [Fact]
    public async Task Get_urls_should_return_all_created_urls()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://google.com",
                null,
                null));

        await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://github.com",
                null,
                null));

        // Act
        var response = await _client.GetAsync("/api/urls");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<
            PagedResponse<UrlResponse>>();

        result.Should().NotBeNull();
        result!.Items.Should().HaveCount(2);

        result.Items.Should().Contain(x => x.OriginalUrl == "https://google.com");
        result.Items.Should().Contain(x => x.OriginalUrl == "https://github.com");
    }

    [Fact]
    public async Task Get_urls_should_support_pagination()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        for (var i = 1; i <= 15; i++)
        {
            await _client.PostAsJsonAsync(
                "/api/urls",
                new CreateShortUrlCommand(
                    $"https://site{i}.com",
                    null,
                    null));
        }

        // Act
        var response = await _client.GetAsync(
            "/api/urls?pageNumber=1&pageSize=10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<
            PagedResponse<UrlResponse>>();

        result.Should().NotBeNull();

        result!.Items.Should().HaveCount(10);
        result.Pagination.TotalRecords.Should().Be(15);
    }
}