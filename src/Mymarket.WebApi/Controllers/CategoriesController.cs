using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Categories.Commands.Add;
using Mymarket.Application.Features.Categories.Commands.Delete;
using Mymarket.Application.Features.Categories.Commands.Edit;
using Mymarket.Application.Features.Categories.Queries.Get;
using Mymarket.Application.Features.Categories.Queries.GetAll;
using Mymarket.Application.Features.Categories.Queries.GetById;
using Mymarket.Application.Features.Categories.Queries.GetByIdWithChildren;
using Mymarket.Application.Features.Categories.Queries.GetFlat;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/Categories")]
public class CategoriesController(IMediator _mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var result = await _mediator.Send(new GetCategoriesQuery());
        return Ok(result);
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());
        return Ok(result);
    }

    [HttpGet]
    [Route("GetById")]
    public async Task<IActionResult> GetAllCategories([FromQuery] GetCategoryByIdQuery getCategoryByIdQuery)
    {
        var result = await _mediator.Send(getCategoryByIdQuery);

        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [Route("GetByIdWithChildren")]
    public async Task<IActionResult> GetByIdWithChildren([FromQuery] GetCategoriesByIdWithChildrenQuery getCategoriesByIdWithChildrenQuery)
    {
        var result = await _mediator.Send(getCategoriesByIdWithChildrenQuery);

        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [Route("GetFlat")]
    public async Task<IActionResult> GetFlatCategories()
    {
        var result = await _mediator.Send(new GetFlatCategoriesQuery());

        if (result is null) return NotFound();

        return Ok(result);
    }

    [HttpPatch]
    [Route("Edit")]
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
