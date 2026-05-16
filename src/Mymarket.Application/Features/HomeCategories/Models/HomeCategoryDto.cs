using Mapster;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.HomeCategories.Models;

public class HomeCategoryDto : IMapFrom<HomeCategoriesEntity>
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int Order { get; set; }
    public string? LogoUrl { get; set; } = null;
    public string? Name { get; set; } = null;
}
