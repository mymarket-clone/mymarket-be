using AutoMapper;
using Mymarket.Domain.Constants;
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
    public string Description { get; set; } = null!;
    public string? DescriptionEn { get; set; }
    public string? DescriptionRu { get; set; }
    public bool ForDisabledPerson { get; set; }
    public bool IsColored { get; set; }
    public bool IsNegotiable { get; set; }
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public PostType PostType { get; set; }
    public double Price { get; set; }
    public PromoType? PromoType { get; set; }
    public byte SalePercentage { get; set; }
    public string Title { get; set; } = null!;
    public string? TitleEn { get; set; }
    public string? TitleRu { get; set; }

    public sealed class Mapping : Profile
    {
        public Mapping() 
        {
            CreateMap<PostEntity, PostDto>();
        }
    }
}
