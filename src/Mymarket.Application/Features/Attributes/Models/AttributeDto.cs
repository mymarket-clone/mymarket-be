using Mymarket.Domain.Enums;
using Mymarket.Domain.Entities;
using Mapster;

namespace Mymarket.Application.Features.Attributes.Models;

public class AttributeDto : IMapFrom<AttributeEntity>
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
    public AttributeType AttributeType { get; set; }
    public int? UnitId { get; set; }
}
