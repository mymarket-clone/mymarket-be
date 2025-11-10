using FluentValidation;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Users.Commands.LoginUser;

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
