using Mymarket.Domain.Common;
using Mymarket.Domain.Enums;
using System.Data;

namespace Mymarket.Domain.Entities;

public class UserEntity : BaseEntity<int>
{
    public required string Firstname { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required GenderType Gender { get; set; }
    public required int BirthYear { get; set; }
    public string? PhoneNumber {  get; set; }
    public string? PasswordHash { get; set; }
    public bool EmailVerified { get; set; }
    public bool IsBlocked { get; set; }
    public decimal Balance { get; set; }
    public string? RefreshToken { get; set; }   
    public AccessLevelType AccessLevel { get; set; } = AccessLevelType.User;
    public DateTime RefreshTokenExpiry { get; set; }
    public ICollection<RoleEntity> Roles { get; set; } = [];
    public ICollection<PermissionEntity> Permissions { get; set; } = [];
    public ICollection<PostEntity> Posts { get; set; } = [];
    public ICollection<PostViewEntity> PostViews { get; set; } = [];
    public ICollection<UserExternalLoginEntity> ExternalLogins { get; set; } = [];
}
