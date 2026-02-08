using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Users.Commands.RefreshUser;
public class RefreshUserCommandValidator : AbstractValidator<RefreshUserCommand>
{
    private readonly IApplicationDbContext _context;

    public RefreshUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Valid refresh token is required")
            .MustAsync(TokenExistsAndValid).WithMessage("Refresh token is invalid or expired");
    }

    private async Task<bool> TokenExistsAndValid(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);

        if (user == null) return false;

        return user.RefreshTokenExpiry > DateTime.UtcNow;
    }
}
