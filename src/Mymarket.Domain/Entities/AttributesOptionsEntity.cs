using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class AttributesOptionsEntity : BaseEntity<int>
{
    public required int AttributeId { get; set; }
    public AttributeEntity? Attribute { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
    public required int Order { get; set; }
    public ICollection<PostAttributesEntity> PostAttributes { get; set; } = [];
}
