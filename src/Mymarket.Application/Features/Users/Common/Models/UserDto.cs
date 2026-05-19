using Mymarket.Domain.Enums;

namespace Mymarket.Application.features.Users.Common.Models;

public record UserDto(
    int Id,
    string Name,
    string Lastname,
    string Email,
    bool EmailVerified,
    int FavoritesCount,
    string Number,
    GenderType GenderType,
    int BirthYear,
    bool IsBlocked
);
