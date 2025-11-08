using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class User : BaseEntity<int>
{
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required long PhoneNumber {  get; set; }
    public required string PasswordHash { get; set; }
}
