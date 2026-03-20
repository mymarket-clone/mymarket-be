using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class HomeCategoriesEntity : BaseEntity<int>
{
    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; } = default!;
    public int Order { get; set; }
}
