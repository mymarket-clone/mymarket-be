using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Posts.Commands.Add;
using Mymarket.Application.Features.Posts.Queries.GetById;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Authorize]
[Route("api/posts")]
public class PostsController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> AddPost([FromForm] AddPostCommand command)
    {
        await mediator.Send(command);
        return Created();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetPostByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }
}
