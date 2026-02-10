using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.features.Users.Commands.SendEmailVerificationCode;

public class SendEmailVerificationCodeValidator : AbstractValidator<SendEmailVerificationCodeCommand>
{
    private readonly IApplicationDbContext _context;

    public SendEmailVerificationCodeValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(SharedResources.EmailRequired)
            .EmailAddress().WithMessage(SharedResources.InvalidEmail)
            .MustAsync(EmailDoesNotExist).WithMessage(SharedResources.UserWithEmailDoesNotExist)
            .MustAsync(EmailNotVerified).WithMessage(SharedResources.EmailAlreadyVerified);
            //.MustAsync(NoActiveCode).WithMessage(SharedResources.CodeAlreadySent);
    }

    private async Task<bool> EmailDoesNotExist(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(x => x.Email.Equals(email), cancellationToken);
    }

    private async Task<bool> EmailNotVerified(string email, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken);
        return user != null && !user.EmailVerified;
    }

    //private async Task<bool> NoActiveCode(string email, CancellationToken cancellationToken)
    //{
    //    var user = await _context.Users
    //        .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    //    if (user == null) return true;

    //    var activeCode = await _context.VerificationCode
    //        .AnyAsync(x =>
    //            x.UserId == user.Id &&
    //            x.CodeType == CodeType.EmailVerification &&
    //            x.ExpiresAt > DateTime.UtcNow,
    //            cancellationToken);

    //    return !activeCode;
    //}
}
