using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class AttributesOptionsEntity : BaseEntity<int>
{
    public required int AttributeId { get; set; }
    public required AttributesEntity Attribute { get; set; }
    public required string Label { get; set; }
    public required int Order { get; set; }
    public ICollection<PostAttributesEntity> PostAttributes { get; set; } = [];
}
