using Mymarket.Domain.Common;
using Mymarket.Domain.Enums;

namespace Mymarket.Domain.Entities;

public class PostEntity : BaseEntity<int>
{
    public PostType PostType { get; set; }
    public int CategoryId { get; set; }
    public string? YoutubeLink { get; set; }
    public ConditionType ConditionType { get; set; }
    public CategoryEntity? Category { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? TitleEn { get; set; }
    public string? DescriptionEn { get; set; }
    public string? TitleRu { get; set; }
    public string? DescriptionRu { get; set; }
    public bool ForDisabledPerson { get; set; } = false;
    public double? Price { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public byte SalePercentage { get; set; }
    public bool CanOfferPrice { get; set; } = false;
    public bool IsNegotiable { get; set; } = false;
    public int CityId { get; set; }
    public CityEntity? City { get; set; }
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    public int UserId { get; set; }
    public UserEntity? User { get; set; }
    public PromoType? PromoType { get; set; }
    public int? PromoDays { get; set; }
    public bool IsColored { get; set; } = false;
    public int? ColorDays { get; set; }
    public bool AutoRenewal { get; set; } = false;
    public int? AutoRenewalOnceIn { get; set; }
    public int? AutoRenewalAtTime { get; set; }
    public int? BrandId { get; set; }
    public PostStatus Status { get; set; } = PostStatus.Active;
    public BrandEntity? Brand { get; set; }
    public ICollection<PostsImagesEntity> PostsImages { get; set; } = [];
    public ICollection<FavoritesEntity> Favorites { get; set; } = [];
    public ICollection<PostAttributesEntity> PostAttributes { get; set; } = [];
    public ICollection<PostViewEntity> PostViews { get; set; } = [];
}
