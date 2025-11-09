using Microsoft.AspNetCore.Identity;
using Mymarket.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Mymarket.Application.Users.Common.Helpers;

public static class CryptoHelper
{
    public static string CreateVerificationCode()
    {
        return RandomNumberGenerator.GetInt32(100000, 1000000).ToString("D6");
    }

    public static string HashVerificationCode(string verificationCode)
    {
        byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(verificationCode));

        return Convert.ToBase64String(hashedBytes);
    }

    public static string HashPassword(string password)
    {
        var hasher = new PasswordHasher<UserEntity>();
        return hasher.HashPassword(null!, password);
    }

    public static bool VerifyPassword(string hashedPassword, string password)
    {
        var hasher = new PasswordHasher<UserEntity>();
        return hasher.VerifyHashedPassword(null!, hashedPassword, password)
            == PasswordVerificationResult.Success;
    }
}
