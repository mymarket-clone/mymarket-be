using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Posts.Commands.Add;
using Mymarket.Application.Features.Posts.Queries.GetById;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/Posts")]
public class PostsController(IMediator _mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> AddPost([FromForm] AddPostCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetPostById([FromQuery] int id)
    {
        var result = await _mediator.Send(new GetPostByIdQuery(id));

        if (result is null) return NotFound();
        return Ok(result);
    }
}
