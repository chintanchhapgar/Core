namespace UrlShortener.Application.Common.Responses;

public sealed class ApiResponse
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;
}