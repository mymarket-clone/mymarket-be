using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Cities.Queries;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/cities")]
[ApiController]
public class CitiesController(IMediator _mediator) : BaseController
{

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllCategories()
    {
        var result = await _mediator.Send(new GetAllCitiesQuery());
        return Ok(result);
    }
}
