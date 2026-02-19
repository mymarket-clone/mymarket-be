using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mymarket.Infrastructure.Authentication;

internal sealed class TokenProvider(JwtOptions jwtOptions) : ITokenProvider
{
    public (string, DateTime) CreateAccessToken(UserModel user)
    { 
        var claims = new List<Claim>
        {
            new(Domain.Constants.ClaimTypes.Id, user.Id.ToString()),
            new(Domain.Constants.ClaimTypes.Name, user.Name.ToString()),
            new(Domain.Constants.ClaimTypes.Email, user.Email.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


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
