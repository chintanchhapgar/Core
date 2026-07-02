using FluentValidation;

namespace UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;

public sealed class CreateShortUrlCommandValidator
    : AbstractValidator<CreateShortUrlCommand>
{
    public CreateShortUrlCommandValidator()
    {
        RuleFor(x => x.OriginalUrl)
            .NotEmpty()
            .Must(BeValidUrl)
            .WithMessage("A valid HTTP or HTTPS URL is required.");

        RuleFor(x => x.CustomAlias)
            .MaximumLength(30)
            .Matches("^[a-zA-Z0-9_-]*$")
            .When(x => !string.IsNullOrWhiteSpace(x.CustomAlias))
            .WithMessage("Alias may contain only letters, numbers, '-' and '_'.");
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp
                   || uri.Scheme == Uri.UriSchemeHttps);
    }
}