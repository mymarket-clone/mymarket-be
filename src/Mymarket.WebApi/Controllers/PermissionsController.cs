using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Permissions.Queries.Get;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/permissions")]
public class PermissionsController(IMediator mediator) : BaseController
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPermissions()
    {
        var result = await mediator.Send(new GetPermissionsQuery());
        return Ok(result);
    }
}
