using AutoMapper;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.features.Categories.Models;

public class CategoryTreeAllDto : ITreeNode<CategoryTreeAllDto>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
    public List<CategoryTreeAllDto>? Children { get; set; }
    public sealed class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CategoryEntity, CategoryTreeAllDto>();
        }
    }
}
