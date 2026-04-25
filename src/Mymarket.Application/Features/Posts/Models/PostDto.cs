using AutoMapper;
using Mymarket.Domain.Enums;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Posts.Models;

public class PostDto
{
    public int Id { get; set; }
    public bool AutoRenewal { get; set; }
    public bool CanOfferPrice { get; set; }
    public int CategoryId { get; set; }
    public ConditionType ConditionType { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public string? Description { get; set; }
    public bool ForDisabledPerson { get; set; }
    public bool IsColored { get; set; }
    public bool IsNegotiable { get; set; }
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public PostType PostType { get; set; }
    public double? Price { get; set; }
    public PromoType? PromoType { get; set; }
    public byte SalePercentage { get; set; }
    public double? PriceAfterDiscount { get; set; }
    public required string Title { get; set; }
    public int? BrandId { get; set; }
    public List<string> Images { get; set; } = [];
    public bool IsFavorite { get; set; }

    public sealed class Mapping : Profile
    {
        public Mapping() 
        {
            CreateMap<PostEntity, PostDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src =>
                    src.PostsImages
                        .Where(pi => pi.Image != null && pi.Image.Url != null)
                        .OrderBy(pi => pi.Order)
                        .OrderBy(pi => pi.Order)
                        .Select(pi => pi.Image!.Url)
                ))
                .ForMember(dest => dest.PriceAfterDiscount, opt => opt.MapFrom(src =>
                    src.Price.HasValue
                        ? src.Price.Value * (1 - src.SalePercentage / 100.0)
                        : (double?)null
                ));
        }
    }
}
