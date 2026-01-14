using AutoMapper;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.features.Categories.Models;

public class CategoryTreeDto : ITreeNode<CategoryTreeDto>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<CategoryTreeDto>? Children { get; set; }
    public sealed class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CategoryEntity, CategoryTreeDto>();
        }
    }
}
