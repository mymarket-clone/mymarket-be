using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class PostViewEntity
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public PostEntity Post { get; set; } = null!;
    public int? UserId { get; set; }
    public UserEntity? User { get; set; }
    public Guid? SessionId { get; set; }
    public DateOnly ViewDate { get; set; }
    public DateTime ViewedAt { get; set; }
}
