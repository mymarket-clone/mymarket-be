using Mymarket.Domain.Constants;

namespace Mymarket.Application.Features.CategoryAttributes.Models;

public class CategoryAttributeOptionsDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int AttributeId { get; set; }
    public required string AttributeName { get; set; }
    public AttributeType AttributeType { get; set; }
    public bool IsRequired { get; set; }
    public int Order { get; set; }
    public List<AttributeOptionDto>? Options { get; set; } = [];
}
