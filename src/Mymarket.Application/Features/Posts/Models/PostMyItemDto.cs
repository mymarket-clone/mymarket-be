using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Posts.Models;

public class PostMyItemDto
{ 
    public int Id { get; set; }
    public required string Title { get; set; }
    public double Price { get; set; }
    public bool IsNegotiable { get; set; }
    public required string ImageUrl { get; set; }
    public int ViewsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public PostStatus Status { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public double? PriceAfterDiscount { get; set; }
}
