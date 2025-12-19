using Mymarket.Domain.Common;
using Mymarket.Domain.Constants;

namespace Mymarket.Domain.Entities;

public class VerificationCodeEntity : BaseEntity<int>
{
    public required int UserId { get; set; }
    public required string CodeHash { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public required CodeType CodeType { get; set; }
    public required bool IsVerified { get; set; }
    public UserEntity? User { get; set; }
}
