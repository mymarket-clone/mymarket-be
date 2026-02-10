using Mymarket.Domain.Common;
using Mymarket.Domain.Constants;

namespace Mymarket.Domain.Entities;

public class AttributesEntity : BaseEntity<int>
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string NameEn { get; set; }
    public required string NameRu { get; set; }
    public required bool IsRequired { get; set; } = false;
    public required AttributeType AttributeType { get; set; }
    public ICollection<PostAttributesEntity> PostAttributes { get; set; } = [];
}
