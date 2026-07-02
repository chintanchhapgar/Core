using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Admin.DeleteUrl;

public sealed class DeleteUrlCommandHandler
    : ICommandHandler<DeleteUrlCommand>
{
    private readonly IShortUrlRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUrlCommandHandler(
        IShortUrlRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteUrlCommand request,
        CancellationToken cancellationToken)
    {
        var url = await _repository.GetByIdAsync(
            request.UrlId,
            cancellationToken);

        if (url is null)
            throw new KeyNotFoundException("URL not found.");

        _repository.Delete(url);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}