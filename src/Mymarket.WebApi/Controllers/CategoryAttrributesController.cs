using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.CategoryAttributes.Commands.Add;
using Mymarket.Application.Features.CategoryAttributes.Commands.Delete;
using Mymarket.Application.Features.CategoryAttributes.Commands.Edit;
using Mymarket.Application.Features.CategoryAttributes.Queries.Get;
using Mymarket.Application.Features.CategoryAttributes.Queries.GetById;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/CategoryAttributes")]
public class CategoryAttrributesController(IMediator _mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCategoryAttribute([FromQuery] int id)
    {
        var result = await _mediator.Send(new GetCategoryAttributesQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategoryAttribute(AddCategoryAttributesCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditCategoryAttribute(
        [FromRoute] int Id,
        [FromBody] EditCategoryAttributesCommand command)
    {
        await _mediator.Send(command with { Id = Id });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoryAttribute(int id)
    {
        await _mediator.Send(new DeleteCategoryAttributesCommand(id));
        return NoContent();
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetCategoryAttributeById([FromQuery] int id)
    {
        var result = await _mediator.Send(new GetCategoryAttributeByIdQuery(id));

        if (result is null) return NotFound();
        return Ok(result);
    }
}
