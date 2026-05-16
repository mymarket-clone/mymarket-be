using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Categories.Models;

public class CategoryDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; } = null;
    public required string Name { get; set; }
    public string? NameEn { get; set; } = null;
    public string? NameRu { get; set; } = null;
    public bool HasChildren { get; set; }
    public bool BrandRequired { get; set; } = false;
    public bool BrandVisible { get; set; } = false;
    public CategoryPostType CategoryPostType { get; set; }
    public string? LogoUrl { get; set; } = null; 
}
