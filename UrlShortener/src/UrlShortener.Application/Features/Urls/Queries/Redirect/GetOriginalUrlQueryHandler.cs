using MediatR;
using UrlShortener.Domain.Interfaces;

namespace UrlShortener.Application.Features.Urls.Queries.Redirect;

public sealed class GetOriginalUrlQueryHandler
    : IRequestHandler<GetOriginalUrlQuery, string?>
{
    private readonly IShortUrlRepository _repository;
    private readonly IUnitOfWork _unitOfWork;


    public GetOriginalUrlQueryHandler(
        IShortUrlRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> Handle(
        GetOriginalUrlQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByShortCodeAsync(
            request.ShortCode,
            cancellationToken);

        if (entity is null)
            return null;

        entity.RegisterClick();

        _repository.Update(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return entity.OriginalUrl;
    }
}