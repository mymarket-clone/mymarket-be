using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class EmailVerificationEntity : BaseEntity<int>
{
    public required int UserId { get; set; }
    public required string CodeHash { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public UserEntity? User { get; set; }
}
