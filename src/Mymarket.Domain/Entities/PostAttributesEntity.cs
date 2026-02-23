using Mymarket.Domain.Common;
using Mymarket.Domain.Constants;

namespace Mymarket.Domain.Entities;

public class PostAttributesEntity : BaseEntity<int>
{
    public required int PostId { get; set; }
    public PostEntity? Post { get; set; }
    public required int AttributeId { get; set; }
    public AttributeEntity? Attribute { get; set; }
    public required string Value { get; set; }
    public required AttributeType ValueType { get; set; }
}
