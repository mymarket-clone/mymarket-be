using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class PostAttributesEntity : BaseEntity<int>
{
    public required int PostId { get; set; }
    public required PostEntity Post { get; set; }
    public required int AttributeId { get; set; }
    public required AttributesEntity Attribute { get; set; }
    public int? OptionId { get; set; }
    public AttributesOptionsEntity? Option { get; set; }
    public string? ValueText { get; set; }
    public int? ValueNumber { get; set; }
}
