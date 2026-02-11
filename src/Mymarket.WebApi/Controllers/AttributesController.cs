using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Attributes.Commands.Add;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/Attributes")]
public class AttributesController(IMediator _mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> AddAttribute(AddAttributeCommand AddAttributeCommand)
    {
        await _mediator.Send(AddAttributeCommand);
        return NoContent();
    }
}
