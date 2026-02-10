using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Models;

namespace Mymarket.Application.Features.Users.Commands.RefreshUser;

public record RefreshUserCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<AuthDto>;

public class RefreshUserCommandHandler(
    IApplicationDbContext _context,
    ITokenProvider _tokenProvider) : IRequestHandler<RefreshUserCommand, AuthDto>
{
    public async Task<AuthDto> Handle(RefreshUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.RefreshToken == request.RefreshToken && x.RefreshTokenExpiry > DateTime.UtcNow,
                cancellationToken);

        if (user is null)
            throw new UnauthorizedAccessException(SharedResources.InvalidRefreshOrUser);

        var userModel = new UserModel
        {
            Id = user!.Id,
            Name = user!.Firstname,
            Lastname = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Password = user.PasswordHash,
            EmailVerified = user.EmailVerified,
        };

        var (accessToken, accessTokenTtl) = _tokenProvider.CreateAccessToken(userModel!);
        var (refreshToken, refreshTokenTtl) = _tokenProvider.CreateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = refreshTokenTtl;

        await _context.SaveChangesAsync(cancellationToken);

        return new AuthDto
        (
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresAt: accessTokenTtl,
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
