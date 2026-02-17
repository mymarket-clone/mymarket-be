namespace Mymarket.Application.Features.CategoryAttributes.Models;

public sealed class AttributeOptionDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Order { get; set; }
}
