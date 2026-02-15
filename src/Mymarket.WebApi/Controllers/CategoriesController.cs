using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Categories.Commands.Add;
using Mymarket.Application.Features.Categories.Commands.Delete;
using Mymarket.Application.Features.Categories.Commands.Edit;
using Mymarket.Application.Features.Categories.Queries.GetAllFlat;
using Mymarket.Application.Features.Categories.Queries.GetById;
using Mymarket.Application.Features.Categories.Queries.GetFlat;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/Categories")]
public class CategoriesController(IMediator _mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _mediator.Send(new GetCategoriesQuery());

        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditCategory(
        [FromRoute] int id, 
        [FromBody] EditCategoryCommand command)
    {
        await _mediator.Send(command with { Id = id });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetCategoryById([FromQuery] int id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));

        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());

        if (result is null) return NotFound();
        return Ok(result);
    }
}
