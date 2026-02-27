using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Units.Commands.Add;
using Mymarket.Application.Features.Units.Commands.Delete;
using Mymarket.Application.Features.Units.Commands.Edit;
using Mymarket.Application.Features.Units.Queries.GetAll;
using Mymarket.Application.Features.Units.Queries.GetById;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/Units")]
public class UnitsController(IMediator mediator) : BaseController
{
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

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllUnit([FromQuery] GetAllUnitQuery GetAllUnitQuery)
    {
        var result = await mediator.Send(GetAllUnitQuery);

        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetByIdUnit([FromQuery] GetUnitById GetUnitById)
    {
        var result = await mediator.Send(GetUnitById);

        if (result is null) return NotFound();
        return Ok(result);
    }
}
