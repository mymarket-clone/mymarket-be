using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class AttributeUnitEntity : BaseEntity<int>
{
    public required string Name { get; set; }
    public required string NameEn { get; set; }
    public required string NameRu { get; set; }
    public ICollection<AttributeEntity> Attributes { get; set; } = [];
}
    