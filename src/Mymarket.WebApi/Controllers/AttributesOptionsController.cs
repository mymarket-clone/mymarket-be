using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.AttributeOptions.Commands.Add;
using Mymarket.Application.Features.AttributeOptions.Commands.Delete;
using Mymarket.Application.Features.AttributeOptions.Commands.Edit;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers
{
    [Authorize]
    [Route("api/attribute-options")]
    public class AttributesOptionsController(IMediator mediator) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> AddAttributeOption([FromBody] AddAttributeOptionCommand command)
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttributeOption(int id)
        {
            await mediator.Send(new DeleteAttributeOptionCommand(id));
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAttributeOption(
            [FromRoute] int Id,
            [FromBody] EditAttributeOptionCommand command)
        {
            await mediator.Send(command with { Id = Id });
            return NoContent();
        }
    }
}
