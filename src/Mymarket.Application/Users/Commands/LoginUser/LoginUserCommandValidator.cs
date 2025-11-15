using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
            .NotEmpty().WithMessage(SharedResources.EmailOrPhoneRequired)
            .MustAsync(EmailNotVerified).WithMessage(SharedResources.EmailNotVerified);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(SharedResources.PasswordRequired);
    }

    private async Task<bool> EmailNotVerified(string email, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken);
        return user != null && user.EmailVerified;
    }
}
