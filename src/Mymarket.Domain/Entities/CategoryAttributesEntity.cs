using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class CategoryAttributesEntity : BaseEntity<int>
{
    public required int CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
    public required int AttributeId { get; set; }
    public AttributeEntity? Attribute { get; set; }
    public required bool IsRequired { get; set; } = false;
    public required int Order { get; set; }
}
