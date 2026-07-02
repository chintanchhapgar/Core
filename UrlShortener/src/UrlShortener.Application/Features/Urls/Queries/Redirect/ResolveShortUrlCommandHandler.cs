using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Commands.ResolveShortUrl;

public sealed class ResolveShortUrlCommandHandler
    : ICommandHandler<ResolveShortUrlCommand, string?>
{
    private readonly IShortUrlRepository _repository;
    private readonly IRequestInfoProvider _requestInfoProvider;

    public ResolveShortUrlCommandHandler(
        IShortUrlRepository repository,
        IRequestInfoProvider requestInfoProvider)
    {
        _repository = repository;
        _requestInfoProvider = requestInfoProvider;
    }

    public async Task<string?> Handle(
        ResolveShortUrlCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByShortCodeAsync(
            request.ShortCode,
            cancellationToken);

        if (entity is null)
            return null;

        // Prevent redirect if disabled
        if (!entity.IsActive)
        {
            throw new InvalidOperationException(
                "This short URL has been deactivated.");
        }

        // Prevent redirect if expired
        if (entity.IsExpired())
        {
            throw new InvalidOperationException(
                "This short URL has expired.");
        }

        entity.RegisterClick();

        var requestInfo = _requestInfoProvider.GetCurrentRequest();

        var visit = new ShortUrlVisit(
            entity.Id,
            requestInfo.IpAddress,
            requestInfo.UserAgent,
            requestInfo.Browser,
            requestInfo.OperatingSystem,
            requestInfo.Referrer);

        _repository.Update(entity);

        await _repository.AddVisitAsync(
            visit,
            cancellationToken);

        // TransactionBehavior will call SaveChangesAsync()

        return entity.OriginalUrl;
    }
}