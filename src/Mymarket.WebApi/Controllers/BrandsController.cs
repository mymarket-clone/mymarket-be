using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Brands.Commands.Add;
using Mymarket.Application.Features.Brands.Commands.Delete;
using Mymarket.Application.Features.Brands.Commands.Edit;
using Mymarket.Application.Features.Brands.Queries.Get;
using Mymarket.Application.Features.Brands.Queries.GetById;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/Brands")]
public class BrandsController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetBrands()
    {
        var result = await mediator.Send(new GetBrandsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandByid([FromRoute] int Id)
    {
        var result = await mediator.Send(new GetBrandsByIdQuery(Id));

        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddBrand([FromForm] AddBrandCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditBrand([FromRoute] int id, [FromForm] EditBrandCommand command)
    {
        var result  = await mediator.Send(command with { Id = id });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrand([FromRoute] int Id)
    {
        await mediator.Send(new DeleteBrandCommand(Id));
        return NoContent();
    }
}
