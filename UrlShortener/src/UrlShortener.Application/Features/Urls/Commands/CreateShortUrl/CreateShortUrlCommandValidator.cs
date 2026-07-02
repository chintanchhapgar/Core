using FluentValidation;

namespace UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;

public sealed class CreateShortUrlCommandValidator
    : AbstractValidator<CreateShortUrlCommand>
{
    public CreateShortUrlCommandValidator()
    {
        RuleFor(x => x.OriginalUrl)
            .NotEmpty()
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("A valid URL is required.");

        RuleFor(x => x.CustomAlias)
            .MaximumLength(30)
            .Matches("^[a-zA-Z0-9_-]*$")
            .When(x => !string.IsNullOrWhiteSpace(x.CustomAlias))
            .WithMessage("Alias may contain only letters, numbers, '-' and '_'.");
    }
}