using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Posts.Commands.Add;
using Mymarket.Application.Features.Posts.Queries.GetById;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/posts")]
public class PostsController(IMediator _mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> AddPost(AddPostCommand createPostCommand)
    {
        await _mediator.Send(createPostCommand);

        return Created();
    }

    [HttpPost]
    [Route("GetById")]
    public async Task<IActionResult> GetPostById(GetPostByIdQuery getPostByIdQuery)
    {
        var result = await _mediator.Send(getPostByIdQuery);

        if (result is null) return NotFound();

        return Ok(result);
    }
}
