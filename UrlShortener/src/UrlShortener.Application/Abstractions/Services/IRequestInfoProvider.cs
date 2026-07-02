namespace UrlShortener.Application.Abstractions.Services;

public interface IRequestInfoProvider
{
    RequestInfo GetCurrentRequest();
}