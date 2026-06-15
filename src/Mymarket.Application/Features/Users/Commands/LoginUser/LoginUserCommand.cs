using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Common.Exceptions;
using Mymarket.Application.features.Users.Common.Helpers;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;
using Mymarket.Domain.Models;

namespace Mymarket.Application.features.Users.Commands.LoginUser;

public record LoginUserCommand(
    string EmailOrPhone,
    string Password
) : IRequest<AuthDto>;

public class LoginUserCommandHandler(
    IApplicationDbContext context,
    IAuthSessionService authSessionService) : IRequestHandler<LoginUserCommand, AuthDto>
{
    public async Task<AuthDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(
                x => x.Email.ToLower() == request.EmailOrPhone.ToLower() || x.PhoneNumber == request.EmailOrPhone,
                cancellationToken);

        if (user is null || string.IsNullOrWhiteSpace(user.PasswordHash) || !CryptoHelper.VerifyPassword(user.PasswordHash, request.Password))
        {
            throw new UnauthorizedAccessException(SharedResources.InvalidUserOrPassword);
        }

        if (!user.EmailVerified)
        {
            throw new EmailNotVerifiedException(user.Email);
        }

        return await authSessionService.CreateSessionAsync(user, cancellationToken);
    }
}
