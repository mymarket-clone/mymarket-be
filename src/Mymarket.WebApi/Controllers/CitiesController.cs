using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Cities.Queries;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/cities")]
public class CitiesController(IMediator mediator) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCitites()
    {
        var result = await mediator.Send(new GetCitiesQuery());
        return Ok(result);
    }
}
