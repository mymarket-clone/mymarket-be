using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Units.Commands.Add;
using Mymarket.Application.Features.Units.Commands.Delete;
using Mymarket.Application.Features.Units.Commands.Edit;
using Mymarket.Application.Features.Units.Queries.Get;
using Mymarket.Application.Features.Units.Queries.GetById;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/units")]
public class UnitsController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllUnit()
    {
        var result = await mediator.Send(new GetUnitsQuery());
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdUnit([FromRoute] int id)
    {
        var result = await mediator.Send(new GetUnitByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddUnit(AddUnitCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditUnit(
        [FromRoute] int Id,
        [FromBody] EditUnitCommand command)
    {
        await mediator.Send(command with { Id = Id });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUnit(int id)
    {
        await mediator.Send(new DeleteUnitCommand(id));
        return NoContent();
    }
}
