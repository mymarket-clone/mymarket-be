using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class RoleEntity : BaseEntity<int>
{
    public required string Name { get; set; }
    public ICollection<PermissionEntity> Permissions { get; set; } = [];
    public ICollection<UserEntity> Users { get; set; } = [];
}
