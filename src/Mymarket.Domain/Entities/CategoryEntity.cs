using Mymarket.Domain.Common;
using Mymarket.Domain.Constants;

namespace Mymarket.Domain.Entities;

public class CategoryEntity : BaseEntity<int>
{
    public int? ParentId { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
    public CategoryPostType CategoryPostType { get; set; }
    public CategoryEntity? Parent { get; set; }
    public ICollection<CategoryEntity> Children { get; set; } = [];
}
