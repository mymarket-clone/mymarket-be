using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class ChatEntity : BaseEntity<int>
{
    public int PostId { get; set; }
    public PostEntity Post { get; set; } = null!;
    public int User1Id { get; set; }
    public UserEntity User1 { get; set; } = null!;
    public int User2Id { get; set; }
    public UserEntity User2 { get; set; } = null!;
    public ICollection<ChatMessageEntity> Messages { get; set; } = [];
}
