using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class PostAttributesEntity : BaseEntity<int>
{
    public required int PostId { get; set; }
    public PostEntity? Post { get; set; }
    public required int AttributeId { get; set; }
    public AttributeEntity? Attribute { get; set; }
    public int? OptionId { get; set; }
    public AttributesOptionsEntity? Option { get; set; }
    public string? Value { get; set; }
}
