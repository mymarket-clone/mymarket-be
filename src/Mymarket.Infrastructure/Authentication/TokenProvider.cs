using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mymarket.Infrastructure.Authentication;

internal sealed class TokenProvider(IConfiguration configuration) : ITokenProvider
{
    public (string, DateTime) CreateAccessToken(UserModel user)
    {
        string secretKey = configuration["JwtSettings:SecretKey"]!;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JwtSettings:ExpiryMinutes"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Name.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()),
                new Claim("email_verified", user.EmailVerified.ToString()),
            ]),
            Expires = expiresAt,
            SigningCredentials = credentials,
            Issuer = configuration["JwtSettings:Issuer"],
            Audience = configuration["JwtSettings:Audience"],
        };


        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return (token, expiresAt);
    }

    public string CreateRefreshToken()
    {
        var randomBytes = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return WebEncoders.Base64UrlEncode(randomBytes);
    }
}
