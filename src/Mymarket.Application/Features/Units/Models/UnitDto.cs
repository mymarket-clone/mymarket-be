using Mapster;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Units.Models;

public class UnitDto : IMapFrom<AttributeUnitEntity>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
}
