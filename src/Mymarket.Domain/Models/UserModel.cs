namespace Mymarket.Domain.Models;

public class UserModel
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Lastname { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public bool EmailVerified { get; set; }
}
