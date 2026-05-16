using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Categories.Models;

public class CategoryFlatDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; } = null;
    public required string Name { get; set; }
    public CategoryPostType CategoryPostType { get; set; }
    public bool? BrandRequired { get; set; }
    public string? LogoUrl { get; set; } = null;
}
