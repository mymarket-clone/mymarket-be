using Mymarket.Domain.Common;
using Mymarket.Domain.Constants;

namespace Mymarket.Domain.Entities;
public class PostEntity : BaseEntity<int>
{
    public PostType PostType { get; set; }
    public int CategoryId { get; set; }
    public ConditionType? ConditionType { get; set; }
    public CategoryEntity? Category { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public string? TitleEn { get; set; }
    public string? DescriptionEn { get; set; }
    public string? TitleRu { get; set; }
    public string? DescriptionRu { get; set; }
    public bool ForDisabledPerson { get; set; } = false;
    public double Price { get; set; }
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
    public bool IsColored { get; set; } = false;
    public bool AutoRenewal { get; set; } = false;
}
