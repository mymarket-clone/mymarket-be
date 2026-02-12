using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Attributes.Models;

public class AttributeDto
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
    public AttributeType AttributeType { get; set; }
    public int? UnitId { get; set; }
    public sealed class Mapping : AutoMapper.Profile
    {
        public Mapping()
        {
            CreateMap<AttributeEntity, AttributeDto>();
        }
    }
}
