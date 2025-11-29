using FluentValidation;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Users.Queries.UserExists;
internal class UserExistsQueryValidator : AbstractValidator<UserExistsQuery>
{
    public UserExistsQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(SharedResources.EmailRequired)
            .EmailAddress().WithMessage(SharedResources.InvalidEmail)
            .MaximumLength(256).WithMessage(SharedResources.EmailMaxLength);
    }
}
