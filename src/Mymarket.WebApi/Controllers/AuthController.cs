using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Users.Commands;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/auth")]
public class AuthController(IMediator _mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserCommand registerUserCommand)
    {
        var response = await _mediator.Send(registerUserCommand);

        return Ok(response);
    }
}
