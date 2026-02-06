using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Images.Commands.Upload;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/images")]
public class ImagesController(IMediator _mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Register([FromForm] UploadImageCommand uploadImageCommand)
    {
        await _mediator.Send(uploadImageCommand);
        return Created();
    }
}
