using FluentValidation;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Application.Users.Commands;

namespace Mymarket.Application.Users.Validators;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    private readonly IApplicationDbContext _context;

    public LoginUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.EmailOrPhone)
            .NotEmpty().WithMessage(SharedResources.EmailOrPhoneRequired);

        RuleFor(x => x.password)
            .NotEmpty().WithMessage(SharedResources.PasswordRequired);
    }
}
