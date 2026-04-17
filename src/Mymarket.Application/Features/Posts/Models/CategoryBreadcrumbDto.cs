namespace Mymarket.Application.Features.Posts.Models;

public class CategoryBreadcrumbDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool HasChildren { get; set; } 
}