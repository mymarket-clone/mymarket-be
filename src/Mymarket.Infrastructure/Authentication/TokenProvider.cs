using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mymarket.Infrastructure.Authentication;

internal sealed class TokenProvider(IConfiguration _configuration) : ITokenProvider
{
    public (string, DateTime) CreateAccessToken(UserModel user)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"]!;
        var issuer = _configuration["JwtSettings:Issuer"]!;
        var audience = _configuration["JwtSettings:Audience"]!;
        var accessTokenTtl = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:AccessTokenTtl"));

        var claims = new[]
        {
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Name),
            new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
            new Claim("email_verified", user.EmailVerified.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: accessTokenTtl,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenString, accessTokenTtl);
    }

    public (string, DateTime) CreateRefreshToken()
    {
        var randomBytes = new byte[32];
        var refreshTokenTtl = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("JwtSettings:RefreshTokenTtl"));

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return (WebEncoders.Base64UrlEncode(randomBytes), refreshTokenTtl);
    }
}
