using AutoMapper;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Categories.Models;

public class CategoryDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; } = null;
    public required string Name { get; set; }
    public string? NameEn { get; set; } = null;
    public string? NameRu { get; set; } = null;
    public bool HasChildren { get; set; }
    public sealed class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CategoryEntity, CategoryDto>()
                .ForMember(
                    dest => dest.HasChildren,
                    opt => opt.MapFrom(src => src.Children.Any())
                );
        }
    }
}
