using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Application.Users.Common.Helpers;

namespace Mymarket.Application.Users.Commands.PasswordRecovery;

public class PasswordRecoveryCommandValidator : AbstractValidator<PasswordRecoveryCommand>
{
    private readonly IApplicationDbContext _context;

    public PasswordRecoveryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(SharedResources.VerificationCodeRequired)
            .MustAsync(CodeIsValid).WithMessage(SharedResources.CodeInvalidOrExpired);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(SharedResources.PasswordRequired)
            .MinimumLength(8).WithMessage(SharedResources.PasswordMinLength)
            .MaximumLength(64).WithMessage(SharedResources.PasswordMaxLength)
            .Matches(@"^\S+$").WithMessage(SharedResources.PasswordNoSpaces)
            .Matches(@"[A-Z]").WithMessage(SharedResources.PasswordUppercase)
            .Matches(@"\d").WithMessage(SharedResources.PasswordNumber)
            .Matches(@"[\W_]").WithMessage(SharedResources.PasswordSpecial);

        RuleFor(x => x.PasswordConfirm)
            .NotEmpty().WithMessage(SharedResources.PasswordRequired)
            .Must((command, passwordConfirm) => passwordConfirm == command.Password).WithMessage(SharedResources.PasswordDoesnotMatch);
    }

    private async Task<bool> CodeIsValid(string code, CancellationToken cancellationToken)
    {
        var codeHash = CryptoHelper.HashVerificationCode(code.ToString());

        var codeRecord = await _context.VerificationCode.FirstOrDefaultAsync(
            x => x.CodeType == Domain.Constants.CodeType.PasswordRecovery &&
                 x.ExpiresAt > DateTime.UtcNow &&
                 x.CodeHash.Equals(codeHash),
            cancellationToken
        );

        return codeRecord is not null;
    }
}
