using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public sealed class UserExternalLoginEntity : BaseEntity<Guid>
{
    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public string ProviderUserId { get; set; } = null!;
    public string? ProviderEmail { get; set; }
}
