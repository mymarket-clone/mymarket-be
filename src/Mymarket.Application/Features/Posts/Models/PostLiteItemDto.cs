using AutoMapper;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Posts.Models;

public class PostLiteItemDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public double? Price { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public required List<string> Images { get; set; }

    private sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PostEntity, PostLiteItemDto>()
                .ForMember(
                    d => d.Images,
                    opt => opt.MapFrom(s =>
                        s.PostsImages
                            .Where(pi => pi.Image != null && pi.Image.Url != null)
                            .Select(pi => pi.Image!.Url!)
                            .ToList()
                    )
                );
        }
    }
}
