using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Categories.Commands.Add;
using Mymarket.Application.Features.Categories.Commands.Delete;
using Mymarket.Application.Features.Categories.Commands.Edit;
using Mymarket.Application.Features.Categories.Queries.Get;
using Mymarket.Application.Features.Categories.Queries.GetAll;
using Mymarket.Application.Features.Categories.Queries.GetById;
using Mymarket.Application.Features.Categories.Queries.GetChildren;
using Mymarket.Application.Features.Categories.Queries.GetFlat;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/categories")]
public class CategoriesController(IMediator _mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _mediator.Send(new GetCategoriesQuery());
        return Ok(result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());
        return Ok(result);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetCategoryById([FromQuery] int id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));

        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("Children")]
    public async Task<IActionResult> GetChildren([FromQuery] int? id)
    {
        var result = await _mediator.Send(new GetCategoryChildren(id));

        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("GetFlat")]
    public async Task<IActionResult> GetFlatCategories()
    {
        var result = await _mediator.Send(new GetFlatCategoriesQuery());

        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPatch("Edit")]
    public async Task<IActionResult> EditCategory([FromBody] EditCategoryCommand editCategoryCommand)
    {
        await _mediator.Send(editCategoryCommand);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCategory([FromQuery] DeleteCategoryCommand deleteCategoryCommand)
    {
        await _mediator.Send(deleteCategoryCommand);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryCommand addCategoryCommand)
    {
        var result = await _mediator.Send(addCategoryCommand);
        return Ok(result);
    }
}
