using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class ImageEntity : BaseEntity<int>
{
    public required string Url { get; set; }
    public required Guid UniqueId { get; set; }
    public ICollection<PostsImages> PostsImages { get; set; } = [];
}