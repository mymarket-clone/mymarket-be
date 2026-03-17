using AutoMapper;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.CategoryBrands.Models;

public class CategoryBrandDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public required string Name { get; set; }

    public sealed class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<CategoryBrandsEntity, CategoryBrandDto>()
               .ForMember(
                   dest => dest.Name,
                   opt => opt.MapFrom(src => src.Brand!.Name)
               );
        }
    }
}
