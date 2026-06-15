using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;
using Mymarket.Domain.Models;

namespace Mymarket.Application.Services;

public sealed class AuthSessionService(
    IApplicationDbContext context,
    ITokenProvider tokenProvider) : IAuthSessionService
{
    public async Task<AuthDto> CreateSessionAsync(UserEntity user, CancellationToken cancellationToken)
    {
        if (user.IsBlocked)
            throw new UnauthorizedAccessException("Blocked users cannot authenticate.");

        var userModel = new UserModel
        {
            Id = user.Id,
            Name = user.Firstname,
            Lastname = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Password = user.PasswordHash,
            EmailVerified = user.EmailVerified,
        };

        var (accessToken, accessTokenTtl) = tokenProvider.CreateAccessToken(userModel);
        var (refreshToken, refreshTokenTtl) = tokenProvider.CreateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = refreshTokenTtl;

        await context.SaveChangesAsync(cancellationToken);

        var favoritesCount = await context.Favorites.CountAsync(x => x.UserId == user.Id, cancellationToken);

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
                EmailVerified: user.EmailVerified,
                FavoritesCount: favoritesCount,
                Number: user.PhoneNumber,
                GenderType: user.Gender == GenderType.Male ? GenderType.Male : GenderType.Female,
                BirthYear: user.BirthYear,
                IsBlocked: user.IsBlocked,
                Balance: user.Balance
            )
        );
    }
}
