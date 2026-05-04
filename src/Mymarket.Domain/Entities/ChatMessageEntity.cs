using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class ChatMessageEntity : BaseEntity<int>
{
    public int ChatId { get; set; }
    public ChatEntity Chat { get; set; } = null!;
    public int SenderId { get; set; }
    public UserEntity Sender { get; set; } = null!;
    public string Content { get; set; } = null!;
}
