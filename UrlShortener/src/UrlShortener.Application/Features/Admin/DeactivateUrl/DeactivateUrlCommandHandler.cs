using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Admin.DeactivateUrl;

public sealed class DeactivateUrlCommandHandler
    : ICommandHandler<DeactivateUrlCommand>
{
    private readonly IShortUrlRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateUrlCommandHandler(
        IShortUrlRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeactivateUrlCommand request,
        CancellationToken cancellationToken)
    {
        var url = await _repository.GetByIdAsync(
            request.UrlId,
            cancellationToken);

        if (url is null)
            throw new KeyNotFoundException("URL not found.");

        url.Deactivate();

        _repository.Update(url);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}