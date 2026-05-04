using Microsoft.AspNetCore.SignalR;
using Mymarket.Application.Interfaces;

namespace Mymarket.Infrastructure.SignalR.Chat;

public class ChatNotifier(IHubContext<ChatHub> hubContext) : IChatNotifier
{
    public Task SendMessage(string chatId, string message)
    {
        return hubContext.Clients.Group(chatId).SendAsync("ReceiveMessage", message);
    }
}
