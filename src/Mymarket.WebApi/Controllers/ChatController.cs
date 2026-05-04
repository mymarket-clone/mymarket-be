using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Chat.Commands;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/chat")]
public class ChatController(IMediator mediator) : BaseController
{
    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage(CreateChatCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}
