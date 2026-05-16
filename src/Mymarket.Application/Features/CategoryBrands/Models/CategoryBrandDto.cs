using Mapster;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.CategoryBrands.Models;

public class CategoryBrandDto : IMapFrom<CategoryBrandsEntity>
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public required string Name { get; set; }

    public static void Mapping(TypeAdapterConfig config)
    {   
        config.NewConfig<CategoryBrandsEntity, CategoryBrandDto>()
            .Map(dest => dest.Name, src => src.Brand!.Name);
    }
}
