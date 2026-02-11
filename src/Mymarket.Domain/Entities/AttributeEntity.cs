using Mymarket.Domain.Common;
using Mymarket.Domain.Constants;

namespace Mymarket.Domain.Entities;

public class AttributeEntity : BaseEntity<int>
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
    public required AttributeType AttributeType { get; set; }
    public int? UnitId { get; set; }
    public AttributeUnitEntity? Unit { get; set; }
    public ICollection<PostAttributesEntity> PostAttributes { get; set; } = [];
}
