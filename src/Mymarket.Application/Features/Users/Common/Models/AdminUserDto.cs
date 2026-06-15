using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Users.Common.Models;

public record AdminUserDto(
    int Id,
    string Firstname,
    string Lastname,
    string Email,
    GenderType Gender,
    int BirthYear,
    string PhoneNumber,
    bool EmailVerified,
    bool IsBlocked,
    AccessLevelType AccessLevel,
    decimal Balance,
    int PostsCount,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
