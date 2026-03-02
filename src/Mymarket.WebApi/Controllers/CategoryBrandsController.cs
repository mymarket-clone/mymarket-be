using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Categories.Queries.GetById;
using Mymarket.Application.Features.CategoryBrands.Commands.Add;
using Mymarket.Application.Features.CategoryBrands.Commands.AddMultiple;
using Mymarket.Application.Features.CategoryBrands.Commands.Delete;
using Mymarket.Application.Features.CategoryBrands.Commands.Edit;
using Mymarket.Application.Features.CategoryBrands.Queries.Get;
using Mymarket.Application.Features.CategoryBrands.Queries.GetById;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/CategoryBrands")]
public class CategoryBrandsController(
    IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCategoryBrands()
    {
        var result = await mediator.Send(new GetCategoryBrandsQuery());
        return Ok(result);
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetCategoryBrandById([FromQuery] int id)
    {
        var result = await mediator.Send(new GetCategoryBrandByIdQuery(id));

        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategoryBrand(AddCategoryBrandCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("AddMultiple")]
    public async Task<IActionResult> AddMultipleCategoryBrandsCommand(AddMultipleCategoryBrandsCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditCategoryBrand(
        [FromRoute] int id,
        [FromBody] EditCategoryBrandCommand command)
    {
        var result = await mediator.Send(command with { Id = id });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoryBrand([FromRoute] int id)
    {
        var result = await mediator.Send(new DeleteCategoryBrandCommand(id));
        return NoContent();
    }
}
