using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities
{
    public class CategoryEntity : BaseEntity<int>
    {
        public int? ParentId { get; set; }
        public string Name { get; set; } = default!;
        public string? NameEn { get; set; }
        public string? NameRu { get; set; }

        public CategoryEntity? Parent { get; set; }
        public ICollection<CategoryEntity> Children { get; set; } = [];
    }
}
