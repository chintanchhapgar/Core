using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Common.Extensions;

public static class ShortUrlRepositoryExtensions
{
    public static async Task<ShortUrl> GetRequiredAccessibleUrlAsync(
        this IShortUrlRepository repository,
        Guid id,
        bool isAdmin,
        Guid? userId,
        CancellationToken cancellationToken)
    {
        var url = await repository.GetAccessibleUrlAsync(
            id,
            isAdmin,
            userId,
            cancellationToken);

        if (url is null)
            throw new KeyNotFoundException("Short URL not found.");

        return url;
    }
}