namespace Mymarket.Domain.Entities;

public class PermissionEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<RoleEntity> Roles { get; set; } = [];
}
