using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.CategoryAttributes.Commands.Add;
using Mymarket.Application.Features.CategoryAttributes.Commands.Delete;
using Mymarket.Application.Features.CategoryAttributes.Commands.Edit;
using Mymarket.Application.Features.CategoryAttributes.Queries.Get;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/category-attributes")]
public class CategoryAttrributesController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCategoryAttributes([FromQuery] int id)
    {
        var result = await mediator.Send(new GetAttributesQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategoryAttribute(AddCategoryAttributesCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditCategoryAttribute(
        [FromRoute] int Id,
        [FromBody] EditCategoryAttributesCommand command)
    {
        await mediator.Send(command with { Id = Id });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoryAttribute(int id)
    {
        await mediator.Send(new DeleteCategoryAttributesCommand(id));
        return NoContent();
    }
}
