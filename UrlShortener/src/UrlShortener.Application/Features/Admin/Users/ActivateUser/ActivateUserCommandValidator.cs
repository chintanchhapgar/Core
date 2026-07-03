using FluentValidation;

namespace UrlShortener.Application.Features.Admin.Users.ActivateUser;

public sealed class ActivateUserCommandValidator
    : AbstractValidator<ActivateUserCommand>
{
    public ActivateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}