using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.AttributeOptions.Commands.Add;
using Mymarket.Application.Features.AttributeOptions.Commands.Delete;
using Mymarket.Application.Features.AttributeOptions.Commands.Edit;
using Mymarket.Application.Features.AttributeOptions.Queries.GetAllById;
using Mymarket.Application.Features.Attributes.Commands.Delete;
using Mymarket.Application.Features.Units.Commands.Delete;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers
{
    [Authorize]
    [Route("api/AttributeOptions")]
    public class AttributesOptionsController(IMediator _mediator) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> AddAttributeOption([FromBody] AddAttributeOptionCommand AddAttributeOptionCommand)
        {
            var result = await _mediator.Send(AddAttributeOptionCommand);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttributeOption(int id)
        {
            await _mediator.Send(new DeleteAttributeOptionCommand(id));
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAttributeOption(
            [FromRoute] int Id,
            [FromBody] EditAttributeOptionCommand EditAttributeOptionCommand)
        {
            await _mediator.Send(EditAttributeOptionCommand with { Id = Id });
            return NoContent();
        }

        [HttpGet("GetAllById")]
        public async Task<IActionResult> GetByIdAttribute([FromQuery] int id)
        {
            var result = await _mediator.Send(new GetAllAttributeOptionsById(id));
            if (result is null) return NotFound();
            return Ok(result);
        }
    }
}
