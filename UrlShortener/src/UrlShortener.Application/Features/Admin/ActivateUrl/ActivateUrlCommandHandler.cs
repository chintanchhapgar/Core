using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Admin.ActivateUrl;

public sealed class ActivateUrlCommandHandler
    : ICommandHandler<ActivateUrlCommand>
{
    private readonly IShortUrlRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateUrlCommandHandler(
        IShortUrlRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ActivateUrlCommand request,
        CancellationToken cancellationToken)
    {
        var url = await _repository.GetByIdAsync(
            request.UrlId,
            cancellationToken);

        if (url is null)
            throw new KeyNotFoundException("URL not found.");

        url.Activate();

        _repository.Update(url);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}