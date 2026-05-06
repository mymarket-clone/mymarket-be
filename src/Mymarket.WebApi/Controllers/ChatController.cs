using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Chat.Commands.CreateChat;
using Mymarket.Application.Features.Chat.Commands.SendMessage;
using Mymarket.Application.Features.Chat.Query.ChatExists;
using Mymarket.Application.Features.Chat.Query.MyChatsQuery;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/chat")]
public class ChatController(IMediator mediator) : BaseController
{
    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage(CreateChatCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{recieverId}/chat-exists")]
    public async Task<IActionResult> ChatExists([FromRoute] int recieverId)
    {
        bool result = await mediator.Send(new ChatExistsQuery(recieverId));
        return Ok(result);
    }

    [HttpGet("my-chats")]
    public async Task<IActionResult> MyChats()
    {
        var result = await mediator.Send(new MyChatsQuery());
        return Ok(result);
    }

    [HttpPost("{chatId}/message")]
    public async Task<IActionResult> SendMessage([FromRoute] int chatId, SendMessageCommand command)
    {
        await mediator.Send(command with { ChatId = chatId});
        return NoContent();
    }
}  
