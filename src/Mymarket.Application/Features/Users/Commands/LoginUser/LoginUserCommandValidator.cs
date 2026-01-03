using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Users.Common.Helpers;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.features.Users.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    private readonly IApplicationDbContext _context;

    public LoginUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.EmailOrPhone)
            .NotEmpty().WithMessage(SharedResources.EmailOrPhoneRequired)
            .MustAsync(UserExists).WithMessage(SharedResources.InvalidUserOrPassword)
            .MustAsync(PasswordMatches).WithMessage(SharedResources.InvalidUserOrPassword);
    }

    private async Task<bool> UserExists(string emailOrPhone, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Email.ToLower() == emailOrPhone.ToLower() || x.PhoneNumber == emailOrPhone,
                cancellationToken);

        return user != null;
    }

    private async Task<bool> PasswordMatches(LoginUserCommand cmd, string emailOrPhone, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Email.ToLower() == emailOrPhone.ToLower() || x.PhoneNumber == emailOrPhone,
                cancellationToken);

        return user is not null && CryptoHelper.VerifyPassword(user.PasswordHash, cmd.Password);
    }
}