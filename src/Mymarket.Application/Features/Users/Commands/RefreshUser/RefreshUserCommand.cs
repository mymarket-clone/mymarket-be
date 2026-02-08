using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Models;

namespace Mymarket.Application.Features.Users.Commands.RefreshUser;

public record RefreshUserCommand(
    string RefreshToken
) : IRequest<AuthDto>;

public class RefreshUserCommandHandler(
    IApplicationDbContext _context,
    ITokenProvider _tokenProvider) : IRequestHandler<RefreshUserCommand, AuthDto>
{
    public async Task<AuthDto> Handle(RefreshUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken, cancellationToken);

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

        var (accessToken, expiresAt) = _tokenProvider.CreateAccessToken(userModel!);
        var refreshToken = _tokenProvider.CreateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = expiresAt;

        await _context.SaveChangesAsync(cancellationToken);

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
