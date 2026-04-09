using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Users.Common.Models;

public record UserInfoDto(
    int Id,
    string FirstName,
    string Lastname,
    string Email,
    GenderType GenderType,
    int BirthYear,
    string PhoneNumber,
    bool EmailVerified,
    int PostsCount
);