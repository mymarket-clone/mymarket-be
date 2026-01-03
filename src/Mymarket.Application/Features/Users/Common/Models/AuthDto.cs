namespace Mymarket.Application.features.Users.Common.Models;

public record AuthDto(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserDto User);