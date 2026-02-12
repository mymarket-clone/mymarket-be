using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Attributes.Commands.Add;
using Mymarket.Application.Features.Attributes.Commands.Edit;
using Mymarket.Application.Features.Attributes.Queries.GetAll;
using Mymarket.Application.Features.Attributes.Queries.GetById;
using Mymarket.Application.Features.Units.Commands.Delete;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/Attributes")]
public class AttributesController(IMediator _mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> AddAttribute(AddAttributeCommand AddAttributeCommand)
    {
        var result = await _mediator.Send(AddAttributeCommand);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditAttribute(
        [FromRoute] int Id,
        [FromBody] EditAttributeCommand EditAttributeCommand)
    {
        await _mediator.Send(EditAttributeCommand with { Id = Id });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttribute(int id)
    {
        await _mediator.Send(new DeleteUnitCommand(id));
        return NoContent();
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAttribute([FromQuery] GetAllAttributeQuery GetAllAttributeQuery)
    {
        var result = await _mediator.Send(GetAllAttributeQuery);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetByIdAttribute([FromQuery] GetAttributeById GetAttributeById)
    {
        var result = await _mediator.Send(GetAttributeById);
        if (result is null) return NotFound();
        return Ok(result);
    }
}
