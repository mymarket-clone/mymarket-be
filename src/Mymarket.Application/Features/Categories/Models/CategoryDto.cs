using Mymarket.Application.Common.Models;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Categories.Models;

public class CategoryDto : MapFrom<CategoryEntity>
{
    public int Id { get; set; }
    public int? ParentId { get; set; } = null;
    public required string Name { get; set; }
    public string? NameEn { get; set; } = null;
    public string? NameRu { get; set; } = null;
}
