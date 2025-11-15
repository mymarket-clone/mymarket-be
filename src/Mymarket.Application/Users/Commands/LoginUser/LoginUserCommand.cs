using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Application.Users.Common.Dto;
using Mymarket.Application.Users.Common.Helpers;
using Mymarket.Domain.Models;

namespace Mymarket.Application.Users.Commands.LoginUser;

public record LoginUserCommand(string EmailOrPhone, string Password) : IRequest<AuthDto>;

public class LoginUserCommandHandler(IApplicationDbContext _context, ITokenProvider _tokenProvider) : IRequestHandler<LoginUserCommand, AuthDto>
{
    public async Task<AuthDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Email.ToLower().Equals(request.EmailOrPhone.ToLower()) || x.PhoneNumber == request.EmailOrPhone,
                cancellationToken
            );

        if (user is null) throw new UnauthorizedAccessException(SharedResources.InvalidUserOrPassword);

        var isPasswordRight = CryptoHelper.VerifyPassword(user.PasswordHash, request.Password);

        if (!isPasswordRight) throw new UnauthorizedAccessException(SharedResources.InvalidUserOrPassword);

        var userModel = new UserModel
        {
            Name = user.Name,
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
                Name: user.Name,
                Lastname: user.LastName,
                Email: user.Email,
                EmailVerified: user.EmailVerified
            )
        );
    }
}
