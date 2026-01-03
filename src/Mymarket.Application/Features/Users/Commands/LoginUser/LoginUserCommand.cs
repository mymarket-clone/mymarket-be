using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Common.Exceptions;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Models;

namespace Mymarket.Application.features.Users.Commands.LoginUser;

public record LoginUserCommand(string EmailOrPhone, string Password) : IRequest<AuthDto>;

public class LoginUserCommandHandler(IApplicationDbContext _context, ITokenProvider _tokenProvider) : IRequestHandler<LoginUserCommand, AuthDto>
{
    public async Task<AuthDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Email.ToLower() == request.EmailOrPhone.ToLower() || x.PhoneNumber == request.EmailOrPhone,
                cancellationToken);


        if (user is not null && !user.EmailVerified)
        {
            throw new EmailNotVerifiedException(user.Email);
        }

        var userModel = new UserModel
        {
            Name = user!.Firstname,
            Lastname = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Password = user.PasswordHash,
            EmailVerified = user.EmailVerified,
        };

        var (accessToken, expiresAt) = _tokenProvider.CreateAccessToken(userModel);
        var refreshToken = _tokenProvider.CreateRefreshToken();

        return new AuthDto
        (
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresAt: expiresAt,
            User: new UserDto
            (
                Id: user.Id,
                Name: user.Firstname,
                Lastname: user.LastName,
                Email: user.Email,
                EmailVerified: user.EmailVerified
            )
        );
    }
}
