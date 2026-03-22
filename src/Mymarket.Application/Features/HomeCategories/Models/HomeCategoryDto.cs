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
            CreateMap<HomeCategoriesEntity, HomeCategoryDto>();
        }
    }
}
