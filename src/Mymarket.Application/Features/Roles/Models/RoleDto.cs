using Mapster;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Roles.Models;

public class RoleDto : IMapFrom<RoleEntity>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<int> PermissionIds { get; set; } = [];
    public static void Mapping(TypeAdapterConfig config)
    {
        config.NewConfig<RoleEntity, RoleDto>()
            .Map(dest => dest.PermissionIds, src => src.Permissions.Select(permission => permission.Id));
    }
}
