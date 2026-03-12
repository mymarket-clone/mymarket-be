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

namespace Mymarket.WebApi.Controllers;

[Route("api/auth")]
public class AuthController(IMediator mediator) : BaseController
{
    [HttpPost("register-user")]
    public async Task<IActionResult> Register(RegisterUserCommand command)
    {
        await mediator.Send(command);
        return Created();
    }

    [HttpPost("send-email-verification-code")]
    public async Task<IActionResult> SendEmailVerificationCode(SendEmailVerificationCodeCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("verify-email-code")]
    public async Task<IActionResult> VerifyCode(VerifyEmailCodeCommand command)
    {
        var response = await mediator.Send(command);

        return Ok(response);
    }

    [HttpPost("login-user")]
    public async Task<IActionResult> LoginUser(LoginUserCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("send-password-recovery")]
    public async Task<IActionResult> PasswordRecovery(SendPasswordRecoveryCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("verify-password-code")]
    public async Task<IActionResult> VerifyPasswordCode(VerifyPasswordRecoveryCodeCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("password-recovery")]
    public async Task<IActionResult> PasswordRecovery(PasswordRecoveryCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("refresh-user")]
    public async Task<IActionResult> RefreshUser(RefreshUserCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("user-exists")]
    public async Task<IActionResult> UserExists([FromQuery] UserExistsQuery command)
    {
        var result = await mediator.Send(command);

        if (result) return NoContent();
        return NotFound();
    }

}
