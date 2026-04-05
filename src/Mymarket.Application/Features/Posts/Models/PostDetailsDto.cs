using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Posts.Models;

public class PostDetailsDto
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
    public required string Title { get; set; }
    public List<CategoryBreadcrumbDto> Breadcrumb { get; set; } = [];
    public List<PostAttributeDto> Attributes { get; set; } = [];
}
