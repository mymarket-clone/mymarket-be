using Mapster;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Permissions.Queries.Models;

public class PermissionDto : IMapFrom<PermissionEntity>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
