using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Posts.Models;

public class PostAttributeDto
{
    public required string Attribute { get; set; }
    public string? Value { get; set; }
    public AttributeType? AttributeType { get; set; }
}
