using Mymarket.Application.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.features.Categories.Models;

public class CategoryTreeDto : MapFrom<CategoryEntity>, ITreeNode<CategoryTreeDto>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<CategoryTreeDto>? Children { get; set; }
}
