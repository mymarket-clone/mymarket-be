namespace Mymarket.Domain.Models;

public class UserModel
{
    public required string Name { get; set; }
    public required string Lastname { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Password { get; set; }
    public bool EmailVerified { get; set; }
}
