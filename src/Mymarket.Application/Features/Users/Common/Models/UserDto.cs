namespace Mymarket.Application.features.Users.Common.Models;

public record UserDto(int Id, string Name, string Lastname, string Email, bool EmailVerified);
