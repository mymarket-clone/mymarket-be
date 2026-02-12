using AutoMapper;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Units.Models;

public class UnitDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
    public sealed class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AttributeUnitEntity, UnitDto>();
        }
    }
}
