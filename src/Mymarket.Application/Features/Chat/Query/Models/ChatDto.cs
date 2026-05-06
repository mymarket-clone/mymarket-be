namespace Mymarket.Application.Features.Chat.Query.Models;

public class ChatDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string LastMessage { get; set; }
    public DateOnly LastMessageDate { get; set; }
}
