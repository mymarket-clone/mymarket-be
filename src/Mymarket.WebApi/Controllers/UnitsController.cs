using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Units.Commands.Add;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/Units")]
public class UnitsController(IMediator _mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> AddAttribute(AddUnitCommand AddUnitCommand)
    {
        await _mediator.Send(AddUnitCommand);
        return NoContent();
    }
}
