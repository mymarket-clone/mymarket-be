using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class UserEntity : BaseEntity<int>
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber {  get; set; }
    public required string PasswordHash { get; set; }
    public bool EmailVerified { get; set; }
}
