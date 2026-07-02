using FluentValidation;

namespace UrlShortener.Application.Features.Urls.Commands.UpdateUrl;

public sealed class UpdateUrlCommandValidator
    : AbstractValidator<UpdateUrlCommand>
{
    public UpdateUrlCommandValidator()
    {
        RuleFor(x => x.UrlId)
            .NotEmpty();

        RuleFor(x => x.OriginalUrl)
            .NotEmpty()
            .Must(url =>
            {
                return Uri.TryCreate(url, UriKind.Absolute, out var uri)
                    && (uri.Scheme == Uri.UriSchemeHttp ||
                        uri.Scheme == Uri.UriSchemeHttps);
            })
            .WithMessage("Please provide a valid HTTP or HTTPS URL.");

        RuleFor(x => x.ExpirationDateUtc)
            .Must(x => x == null || x > DateTime.UtcNow)
            .WithMessage("Expiration date must be in the future.");
    }
}