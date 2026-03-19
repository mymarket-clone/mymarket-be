using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Categories.Commands.Add;
using Mymarket.Application.Features.Categories.Commands.Delete;
using Mymarket.Application.Features.Categories.Commands.Edit;
using Mymarket.Application.Features.Categories.Queries.Get;
using Mymarket.Application.Features.Categories.Queries.GetAttributes;
using Mymarket.Application.Features.Categories.Queries.GetBrands;
using Mymarket.Application.Features.Categories.Queries.GetById;
using Mymarket.Application.Features.Categories.Queries.GetLocalized;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/categories")]
public class CategoriesController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var result = await mediator.Send(new GetCategoriesQuery());

        return Ok(result);
    }

    [HttpGet("get-localized")]
    public async Task<IActionResult> GetCategoriesLocalized()
    {
        var result = await mediator.Send(new GetCategoriesLocalizedQuery());

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetCategoryByIdQuery(id));

        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}/attributes")]
    public async Task<IActionResult> GetCategoryAttributes([FromRoute] int id)
    {
        var result = await mediator.Send(new GetCategoryAttributesQuery(id));

        return Ok(result);
    }

    [HttpGet("{id}/brands")]
    public async Task<IActionResult> GetCategoryBrands([FromRoute] int id)
    {
        var result = await mediator.Send(new GetCategoryBrandsQuery(id));

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromForm] AddCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditCategory(
        [FromRoute] int id,
        [FromForm] EditCategoryCommand command)
    {
        await mediator.Send(command with { Id = id });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        await mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}
