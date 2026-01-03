using Mymarket.Application.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.features.Categories.Models;

public class CategoryTreeAllDto : MapFrom<CategoryEntity>, ITreeNode<CategoryTreeAllDto>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
    public List<CategoryTreeAllDto>? Children { get; set; }
}
