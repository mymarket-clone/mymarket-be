using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.CategoryBrands.Commands.Add;
using Mymarket.Application.Features.CategoryBrands.Commands.AddMultiple;
using Mymarket.Application.Features.CategoryBrands.Commands.Delete;
using Mymarket.Application.Features.CategoryBrands.Commands.Edit;
using Mymarket.Application.Features.CategoryBrands.Commands.Remove;
using Mymarket.Application.Features.CategoryBrands.Queries.Get;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/category-brands")]
public class CategoryBrandsController(
    IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCategoryBrands()
    {
        var result = await mediator.Send(new GetCategoryBrandsQuery(null));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryBrand(int id)
    {
        var result = await mediator.Send(new GetCategoryBrandsQuery(id));
        return Ok(result);
    }

    [HttpPost("link")]
    public async Task<IActionResult> LinkCategoryBrand(AddCategoryBrandCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("unlink")]
    public async Task<IActionResult> UnlinkCategoryBrand(RemoveCategoryBrandCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpPost("add-multiple")]
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
