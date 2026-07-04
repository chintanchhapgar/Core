using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
using UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;
using UrlShortener.Application.Common.Models;
using UrlShortener.IntegrationTests.Authentication;
using UrlShortener.IntegrationTests.Common;

namespace UrlShortener.IntegrationTests.Urls;

public sealed class UpdateUrlTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public UpdateUrlTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Update_url_should_update_original_url()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var createResponse = await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://google.com",
                null,
                null));

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var created = await createResponse.Content
            .ReadFromJsonAsync<CreateShortUrlResponse>();

        created.Should().NotBeNull();

        var request = new UpdateUrlRequest(
            "https://github.com",
            null);

        // Act
        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/urls/{created!.Id}",
            request);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var listResponse = await _client.GetAsync("/api/urls");

        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await listResponse.Content.ReadFromJsonAsync<
            PagedResponse<UrlResponse>>();

        result.Should().NotBeNull();

        result!.Items.Should().Contain(x =>
            x.OriginalUrl == "https://github.com");
    }

    [Fact]
    public async Task Update_url_should_update_expiration_date()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var createResponse = await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://google.com",
                null,
                null));

        var created = await createResponse.Content
            .ReadFromJsonAsync<CreateShortUrlResponse>();

        var expiration = DateTime.UtcNow.AddDays(7);

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/urls/{created!.Id}",
            new UpdateUrlRequest(
                "https://google.com",
                expiration));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var list = await _client.GetFromJsonAsync<
            PagedResponse<UrlResponse>>("/api/urls");

        list!.Items.Single().ExpiresOnUtc.Should().NotBeNull();
        list.Items.Single().ExpiresOnUtc!.Value.Date.Should().Be(expiration.Date);
    }

    [Fact]
    public async Task Update_non_existing_url_should_return_not_found()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/urls/{Guid.NewGuid()}",
            new UpdateUrlRequest(
                "https://github.com",
                null));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_without_authentication_should_return_401()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/urls/{Guid.NewGuid()}",
            new UpdateUrlRequest(
                "https://github.com",
                null));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}