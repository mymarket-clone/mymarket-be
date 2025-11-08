namespace Mymarket.Application.Users.Common;

public record AuthDto(string RefreshToken, string ExpiresAt, UserDto User);