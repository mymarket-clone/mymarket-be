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
public class AuthController(IMediator _mediator) : BaseController
{
    [HttpPost]
    [Route("RegisterUser")]
    public async Task<IActionResult> Register(RegisterUserCommand registerUserCommand)
    {
        await _mediator.Send(registerUserCommand);

        return Created();
    }

    [HttpPost]
    [Route("SendEmailVerificationCode")]
    public async Task<IActionResult> SendEmailVerificationCode(SendEmailVerificationCodeCommand sendVerificationEmailCommand)
    {
        await _mediator.Send(sendVerificationEmailCommand);

        return NoContent();
    }

    [HttpPost]
    [Route("VerifyEmailCode")]
    public async Task<IActionResult> VerifyCode(VerifyEmailCodeCommand verifyEmailCodeCommand)
    {
        var response = await _mediator.Send(verifyEmailCodeCommand);

        return Ok(response);
    }

    [HttpPost]
    [Route("LoginUser")]
    public async Task<IActionResult> LoginUser(LoginUserCommand loginUserCommand)
    {
        var response = await _mediator.Send(loginUserCommand);

        return Ok(response);
    }

    [HttpPost]
    [Route("SendPasswordRecovery")]
    public async Task<IActionResult> PasswordRecovery(SendPasswordRecoveryCommand passwordRecoveryCommand)
    {
        await _mediator.Send(passwordRecoveryCommand);

        return NoContent();
    }

    [HttpPost]
    [Route("VerifyPasswordCode")]
    public async Task<IActionResult> VerifyPasswordCode(VerifyPasswordRecoveryCodeCommand verifyPasswordRecoveryCodeCommand)
    {
        await _mediator.Send(verifyPasswordRecoveryCodeCommand);

        return NoContent();
    }

    [HttpPost]
    [Route("PasswordRecovery")]
    public async Task<IActionResult> PasswordRecovery(PasswordRecoveryCommand passwordRecoveryCommand)
    {
        await _mediator.Send(passwordRecoveryCommand);

        return NoContent();
    }

    [HttpPost]
    [Route("RefreshUser")]
    public async Task<IActionResult> RefreshUser(RefreshUserCommand refreshUserCommand)
    {
        var response = await _mediator.Send(refreshUserCommand);

        return Ok(response);
    }

    [HttpGet]
    [Route("UserExists")]
    public async Task<IActionResult> UserExists([FromQuery] UserExistsQuery userExistsQuery)
    {
        var result = await _mediator.Send(userExistsQuery);

        if (result) return NoContent();

        return NotFound();
    }

}
