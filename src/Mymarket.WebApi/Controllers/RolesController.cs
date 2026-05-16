using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Roles.Commands.Add;
using Mymarket.Application.Features.Roles.Commands.Delete;
using Mymarket.Application.Features.Roles.Commands.Edit;
using Mymarket.Application.Features.Roles.Queries.Get;
using Mymarket.Domain.Enums;
using Mymarket.Infrastructure.Authentication.Policies;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/roles")]
public class RolesController(IMediator mediator) : BaseController
{
    [HttpGet]
    [HasPermission(default, AccessLevelType.SuperAdmin)]
    public async Task<IActionResult> GetRoles(
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 20)
    {
        var result = await mediator.Send(new GetRolesQuery(
            Page,
            PageSize
        ));

        return Ok(result);
    }

    [HttpPost]
    [HasPermission(default, AccessLevelType.SuperAdmin)]
    public async Task<IActionResult> AddRole(AddRoleCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [HasPermission(default, AccessLevelType.SuperAdmin)]
    public async Task<IActionResult> EditRole([FromRoute] int id, EditRoleCommand command)
    {
        var result = await mediator.Send(command with { Id = id });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [HasPermission(default, AccessLevelType.SuperAdmin)]
    public async Task<IActionResult> DeleteRole([FromRoute] int id)
    {
        await mediator.Send(new DeleteRoleCommand(id));
        return NoContent();
    }
}
