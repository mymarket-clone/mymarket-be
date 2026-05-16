using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Enums;
using Mymarket.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mymarket.Infrastructure.Authentication;

internal sealed class TokenProvider(
    JwtOptions jwtOptions,
    IApplicationDbContext context) : ITokenProvider
{
    public (string, DateTime) CreateAccessToken(UserModel user)
    {
        var permissions = context.Users
            .Where(u => u.Id == user.Id)
            .SelectMany(u => u.Roles)
            .SelectMany(r => r.Permissions)
            .Select(p => p.Id)
            .Distinct()
            .ToList();

        var accesslevel = context.Users
            .FirstOrDefault(u => u.Id == user.Id)?.AccessLevel ?? AccessLevelType.User;

        var claims = new List<Claim>
        {
            new(Domain.Constants.ClaimTypes.Id, user.Id.ToString()),
            new(Domain.Constants.ClaimTypes.Name, user.Name.ToString()),
            new(Domain.Constants.ClaimTypes.Email, user.Email.ToString()),
            new(Domain.Constants.ClaimTypes.AccessLevel, ((int)accesslevel).ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        claims.AddRange(
            permissions.Select(p => new Claim(Domain.Constants.ClaimTypes.Permission, p.ToString()))
        );

        var expiresAt = DateTime.UtcNow.AddMinutes(jwtOptions.AccessTokenTtl);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenString, expiresAt);
    }

    public (string, DateTime) CreateRefreshToken()
    {
        var randomBytes = new byte[32];
        var expiresAt = DateTime.UtcNow.AddMinutes(jwtOptions.RefreshTokenTtl);

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return (WebEncoders.Base64UrlEncode(randomBytes), expiresAt);
    }
}
