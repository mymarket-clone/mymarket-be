using Mymarket.Domain.Common;
using Mymarket.Domain.Constants;

namespace Mymarket.Domain.Entities;

public class UserEntity : BaseEntity<int>
{
    public required string Firstname { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required GenderType Gender { get; set; }
    public required int BirthYear { get; set; }
    public required string PhoneNumber {  get; set; }
    public required string PasswordHash { get; set; }
    public bool EmailVerified { get; set; }
    public string? RefreshToken { get; set; }
}
