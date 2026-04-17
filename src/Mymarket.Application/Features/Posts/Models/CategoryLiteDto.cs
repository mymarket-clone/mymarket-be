namespace Mymarket.Application.Features.Posts.Models;

public class CategoryLiteDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Count { get; set; }
    public bool HasChildren { get; set; }
}
