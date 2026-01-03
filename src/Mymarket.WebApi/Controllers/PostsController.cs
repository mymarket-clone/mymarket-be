using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.features.Posts.Commands.CreatePost;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/posts")]
public class PostsController(IMediator _mediator) : BaseController
{
    [HttpPost]
    [Route("CreatePost")]
    public async Task<IActionResult> CreatePost(CreatePostCommand createPostCommand)
    {
        await _mediator.Send(createPostCommand);

        return Created();
    }
}
