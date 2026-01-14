using AutoMapper;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Categories.Models;

public class CategoryFlatDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; } = null;
    public required string Name { get; set; }
    public sealed class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CategoryEntity, CategoryFlatDto>()
                .ForMember(
                    d => d.Name,
                    opt => opt.MapFrom((src, dest, _, context) =>
                    {
                        var lang = context.Items["lang"] as string ?? "ka";
                        return lang == "en" ? src.NameEn ?? src.Name
                             : lang == "ru" ? src.NameRu ?? src.Name
                             : src.Name;
                    })
                );
        }
    }
}
