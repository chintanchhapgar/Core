using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Features.Urls.DTOs;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;

public sealed class CreateShortUrlCommandHandler
    : ICommandHandler<CreateShortUrlCommand, ShortUrlDto>
{
    private readonly IShortUrlRepository _repository;

    public CreateShortUrlCommandHandler(
        IShortUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<ShortUrlDto> Handle(
        CreateShortUrlCommand request,
        CancellationToken cancellationToken)
    {
        var shortCode = Guid.NewGuid()
            .ToString("N")[..8];

        var entity = new ShortUrl(
            request.OriginalUrl,
            shortCode);

        await _repository.AddAsync(entity, cancellationToken);

        return new ShortUrlDto
        {
            Id = entity.Id,
            OriginalUrl = entity.OriginalUrl,
            ShortCode = entity.ShortCode,
            ClickCount = entity.ClickCount
        };
    }
}