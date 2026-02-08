using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class PostsImages : BaseEntity<int>
{
    public int PostId { get; set; }
    public required PostEntity Post { get; set; }
    public int ImageId { get; set; }
    public required ImageEntity Image { get; set; }
    public int Order { get; set; }
}
