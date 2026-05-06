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

    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

        await Clients.Caller.SendAsync("ChatJoined", new
        {
            chatId,
            connectionId = Context.ConnectionId,
            status = "joined"
        });
    }

    public Task LeaveChat(string chatId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}