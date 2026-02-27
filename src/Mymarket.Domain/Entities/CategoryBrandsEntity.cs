using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class CategoryBrandsEntity : BaseEntity<int>
{
    public required int CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
    public required int BrandId { get; set; }
    public BrandEntity? Brand { get; set; }
}
