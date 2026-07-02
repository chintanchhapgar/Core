using System.Security.Cryptography;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;

namespace UrlShortener.Infrastructure.Services;

public sealed class ShortCodeGenerator : IShortCodeGenerator
{
    private static readonly char[] Alphabet =
        "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
        .ToCharArray();

    private readonly IShortUrlRepository _repository;

    public ShortCodeGenerator(IShortUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> GenerateAsync(
        CancellationToken cancellationToken = default)
    {
        while (true)
        {
            var bytes = RandomNumberGenerator.GetBytes(8);

            char[] chars = new char[8];

            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = Alphabet[bytes[i] % Alphabet.Length];
            }

            var code = new string(chars);

            var exists = await _repository.GetByShortCodeAsync(
                code,
                cancellationToken);

            if (exists is null)
                return code;
        }
    }
}