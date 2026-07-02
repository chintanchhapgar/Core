namespace UrlShortener.Application.Common.Exceptions;

public sealed class DuplicateShortCodeException : Exception
{
    public DuplicateShortCodeException(string shortCode)
        : base($"Short code '{shortCode}' already exists.")
    {
    }
}