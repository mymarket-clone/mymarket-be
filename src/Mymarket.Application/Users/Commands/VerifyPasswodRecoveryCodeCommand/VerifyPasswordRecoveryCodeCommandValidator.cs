using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Users.Commands.VerifyPasswodRecoveryCodeCommand;

public class VerifyPasswordRecoveryCodeCommandValidator : AbstractValidator<VerifyPasswordRecoveryCodeCommand>
{

    private readonly IApplicationDbContext _context;

    public VerifyPasswordRecoveryCodeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(SharedResources.VerificationCodeRequired);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(SharedResources.EmailRequired)
            .EmailAddress().WithMessage(SharedResources.InvalidEmail)
            .MustAsync(EmailDoesNotExist).WithMessage(SharedResources.UserWithEmailDoesNotExist)
            .MustAsync(EmailVerified).WithMessage(SharedResources.EmailNotVerified);
    }

    private async Task<bool> EmailDoesNotExist(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(x => x.Email.Equals(email), cancellationToken);
    }

    private async Task<bool> EmailVerified(string email, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        return user != null && user.EmailVerified;
    }
}
