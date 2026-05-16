using AutoMapper;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Roles.Models;

public class RoleDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<int> PermissionIds { get; set; } = [];

    public sealed class Mapper : Profile
    {
        public Mapper() => CreateMap<RoleEntity, RoleDto>()
            .ForMember(
                destination => destination.PermissionIds,
                options => options.MapFrom(source => source.Permissions.Select(permission => permission.Id)));
    }   
}
