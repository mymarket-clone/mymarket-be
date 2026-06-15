using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Pricing.Commands.Edit;
using Mymarket.Application.Features.Pricing.Commands.EditService;
using Mymarket.Application.Features.Pricing.Queries.Calculate;
using Mymarket.Application.Features.Pricing.Queries.Get;
using Mymarket.Application.Features.Pricing.Queries.GetAdmin;
using Mymarket.Domain.Enums;
using Mymarket.Infrastructure.Authentication.Policies;

namespace Mymarket.WebApi.Controllers;

[Route("api/prices")]
[ApiController]
public class PricesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetListingPrices()
    {
        var result = await mediator.Send(new GetListingPricesQuery());
        return Ok(result);
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateListingPrice(CalculateListingPriceQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("admin")]
    [HasPermission(default, AccessLevelType.SuperAdmin)]
    public async Task<IActionResult> AdminGetListingPrices()
    {
        var result = await mediator.Send(new AdminGetListingPricesQuery());
        return Ok(result);
    }

    [HttpPut("{id}")]
    [HasPermission(default, AccessLevelType.SuperAdmin)]
    public async Task<IActionResult> EditListingPrice(
        [FromRoute] int id,
        EditListingServicePriceCommand command)
    {
        await mediator.Send(command with { Id = id });
        return NoContent();
    }

    [HttpPut("services/{serviceType}")]
    [HasPermission(default, AccessLevelType.SuperAdmin)]
    public async Task<IActionResult> EditListingServicePrices(
        [FromRoute] ListingServiceType serviceType,
        EditListingServicePricesCommand command)
    {
        var result = await mediator.Send(command with { ServiceType = serviceType });
        return Ok(result);
    }
}
