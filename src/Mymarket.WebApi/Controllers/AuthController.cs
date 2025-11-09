using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Users.Commands;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/auth")]
public class AuthController(IMediator _mediator) : BaseController
{
    [HttpPost]
    [Route("RegisterUser")]
    public async Task<IActionResult> Register(RegisterUserCommand registerUserCommand)
    {
        await _mediator.Send(registerUserCommand);

        return NoContent();
    }

    [HttpPost]
    [Route("SendVerificationEmail")]
    public async Task<IActionResult> SendVerificationEmail(SendVerificationEmailCommand sendVerificationEmailCommand)
    {
        await _mediator.Send(sendVerificationEmailCommand);

        return NoContent();
    }

    [HttpPost]
    [Route("VerifyCode")]
    public async Task<IActionResult> VerifyCode(VerifyCodeCommand verifyCodeCommand)
    {
        var response = await _mediator.Send(verifyCodeCommand);

        return Ok(response);
    }

    [HttpPost]
    [Route("LoginUser")]
    public async Task<IActionResult> LoginUser(LoginUserCommand loginUserCommand)
    {
        var response = await _mediator.Send(loginUserCommand);

        return Ok(response);
    }
}
