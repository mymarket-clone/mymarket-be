namespace Mymarket.Application.Users.Common.Dto;

public record AuthDto(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserDto User);