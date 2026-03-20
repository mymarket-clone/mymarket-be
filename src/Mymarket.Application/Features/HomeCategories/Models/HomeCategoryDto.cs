using AutoMapper;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.HomeCategories.Models;

public class HomeCategoryDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int Order { get; set; }
    public string? LogoUrl { get; set; } = null;
    public string? Name { get; set; } = null;

    private sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HomeCategoriesEntity, HomeCategoryDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null)
                )
                .ForMember(
                    dest => dest.LogoUrl,
                    opt => opt.MapFrom(src => src.Category != null && src.Category.Logo != null
                        ? src.Category.Logo.Url: null)
                );
        }
    }
}
