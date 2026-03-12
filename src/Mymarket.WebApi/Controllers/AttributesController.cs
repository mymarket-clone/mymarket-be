using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Attributes.Commands.Add;
using Mymarket.Application.Features.Attributes.Commands.Edit;
using Mymarket.Application.Features.Attributes.Queries.Get;
using Mymarket.Application.Features.Attributes.Queries.GetById;
using Mymarket.Application.Features.Attributes.Queries.GetOptions;
using Mymarket.Application.Features.Units.Commands.Delete;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/attributes")]
public class AttributesController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAttributes()
    {
        var result = await mediator.Send(new GetAttributesQuery());
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAttributeById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetAttributeByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id}/options")]
    public async Task<IActionResult> GetAttributeOptions([FromRoute] int id)
    {
        var result = await mediator.Send(new GetAttributeOptionsQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddAttribute(AddAttributeCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditAttribute(
        [FromRoute] int Id,
        [FromBody] EditAttributeCommand command)
    {
        await mediator.Send(command with { Id = Id });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttribute(int id)
    {
        await mediator.Send(new DeleteUnitCommand(id));
        return NoContent();
    }
}
