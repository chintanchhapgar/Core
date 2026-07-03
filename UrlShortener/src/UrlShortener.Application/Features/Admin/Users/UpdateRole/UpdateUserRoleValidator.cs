using FluentValidation;
using UrlShortener.Domain.Constants;

namespace UrlShortener.Application.Features.Admin.Users.UpdateRole;

public sealed class UpdateUserRoleValidator
    : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(x => x == Roles.Admin || x == Roles.User)
            .WithMessage("Role must be either Admin or User.");
    }
}