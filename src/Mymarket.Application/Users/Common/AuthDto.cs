namespace Mymarket.Application.Users.Common;

public record AuthDto(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserDto User);