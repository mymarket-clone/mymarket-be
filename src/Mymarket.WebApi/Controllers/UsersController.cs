using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Users.Queries;
using Mymarket.Application.Features.Users.Queries.GetById;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/users")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id}/phone-number")]
    public async Task<IActionResult> GetPhoneNumber([FromRoute] int id)
    {
        var result = await mediator.Send(new GetPhoneNumber(id));
        return result is null ? NotFound() : Ok(result);
    }
}
