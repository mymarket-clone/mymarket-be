using Mymarket.Domain.Common;
using Mymarket.Domain.Enums;

namespace Mymarket.Domain.Entities;

public class CategoryEntity : BaseEntity<int>
{
    public int? ParentId { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
    public bool? BrandRequired { get; set; } = null;
    public int? LogoId { get; set; } = null;
    public ImageEntity? Logo { get; set; }
    public CategoryPostType CategoryPostType { get; set; }
    public CategoryEntity? Parent { get; set; }
    public ICollection<CategoryEntity> Children { get; set; } = [];
}
