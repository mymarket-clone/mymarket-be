using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.features.Users.Commands.LoginUser;
using Mymarket.Application.features.Users.Commands.PasswordRecovery;
using Mymarket.Application.features.Users.Commands.RegisterUser;
using Mymarket.Application.features.Users.Commands.SendEmailVerificationCode;
using Mymarket.Application.features.Users.Commands.SendPasswordRecoveryCode;
using Mymarket.Application.features.Users.Commands.VerifyEmailCodeCommand;
using Mymarket.Application.features.Users.Commands.VerifyPasswodRecoveryCodeCommand;
using Mymarket.Application.features.Users.Queries.UserExists;
using Mymarket.Application.Features.Users.Commands.RefreshUser;
using Mymarket.WebApi.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mymarket.WebApi.Controllers;

[Route("api/Auth")]
public class AuthController(IMediator _mediator) : BaseController
{
    [HttpPost("RegisterUser")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPost("SendEmailVerificationCode")]
    public async Task<IActionResult> SendEmailVerificationCode(SendEmailVerificationCodeCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("VerifyEmailCode")]
    public async Task<IActionResult> VerifyCode(VerifyEmailCodeCommand command)
    {
        var response = await _mediator.Send(command);

        return Ok(response);
    }

    [HttpPost("LoginUser")]
    public async Task<IActionResult> LoginUser(LoginUserCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("SendPasswordRecovery")]
    public async Task<IActionResult> PasswordRecovery(SendPasswordRecoveryCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("VerifyPasswordCode")]
    public async Task<IActionResult> VerifyPasswordCode(VerifyPasswordRecoveryCodeCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("PasswordRecovery")]
    public async Task<IActionResult> PasswordRecovery(PasswordRecoveryCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("RefreshUser")]
    public async Task<IActionResult> RefreshUser(RefreshUserCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("UserExists")]
    public async Task<IActionResult> UserExists([FromQuery] UserExistsQuery command)
    {
        var result = await _mediator.Send(command);

        if (result) return NoContent();
        return NotFound();
    }

}
