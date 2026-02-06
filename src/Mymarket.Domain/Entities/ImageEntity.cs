using Mymarket.Domain.Common;
using Mymarket.Domain.Constants;

namespace Mymarket.Domain.Entities;

public class ImageEntity : BaseEntity<int>
{
    public required string Fullpath { get; set; }
    public ImageTargetType TargetType { get; set; }
    public int TargetId { get; set; }
    public int? UserId { get; set; }
    public UserEntity? User { get; set; }
}