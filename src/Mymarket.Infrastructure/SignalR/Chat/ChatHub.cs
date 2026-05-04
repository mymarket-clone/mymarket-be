using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Mymarket.Infrastructure.SignalR.Chat;

[Authorize]
public class ChatHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("ReceiveMessage", "Connected to chat");
        await base.OnConnectedAsync();
    }

    public Task JoinChat(string chatId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public Task LeaveChat(string chatId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}