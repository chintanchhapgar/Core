namespace UrlShortener.Application.Abstractions.Services;

public interface IShortCodeGenerator
{
    Task<string> GenerateAsync(
        CancellationToken cancellationToken = default);
}