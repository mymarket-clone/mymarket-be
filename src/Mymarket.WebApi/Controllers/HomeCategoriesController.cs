using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.HomeCategories.Commands.Add;
using Mymarket.Application.Features.HomeCategories.Commands.Delete;
using Mymarket.Application.Features.HomeCategories.Commands.Edit;
using Mymarket.Application.Features.HomeCategories.Commands.Reorder;
using Mymarket.Application.Features.HomeCategories.Queries.Get;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers
{
    [Route("api/home-categories")]
    public class HomeCategoriesController(
        IMediator mediator) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetHomeCategories()
        {
            var result = await mediator.Send(new GetHomeCategoriesQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddHomeCategory(AddHomeCategoryCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("reorder")]
        public async Task<IActionResult> ReorderHomeCategories([FromBody] ReorderHomeCategoriesCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditHomeCategory(
            [FromRoute] int id,
            [FromBody] EditHomeCategoryCommand command)
        {
            var result = await mediator.Send(command with { Id = id});
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHomeCategory([FromRoute] int id)
        {
            await mediator.Send(new DeleteHomeCategoryCommand(id));
            return NoContent();
        }
    }
}
