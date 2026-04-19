using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Posts.Commands.Add;
using Mymarket.Application.Features.Posts.Queries.Get;
using Mymarket.Application.Features.Posts.Queries.GetById;
using Mymarket.Application.Features.Posts.Queries.GetLite;
using Mymarket.Domain.Enums;
using Mymarket.WebApi.Infrastructure;

namespace Mymarket.WebApi.Controllers;

[Route("api/posts")]
public class PostsController(IMediator mediator) : BaseController
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddPost([FromForm] AddPostCommand command)
    {
        await mediator.Send(command);
        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts(
        [FromQuery] int? PriceFrom,
        [FromQuery] int? PriceTo,
        [FromQuery] bool? OfferPrice,
        [FromQuery] bool? Discount,
        [FromQuery] int? LocId,
        [FromQuery] List<ConditionType>? CondType,
        [FromQuery] List<PostType>? PostType,
        [FromQuery] SortType? SortType,
        [FromQuery] bool? ForPsn,
        [FromQuery] int? CatId,
        [FromQuery] int? BrandId,
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 20)
    {
        var result = await mediator.Send(new GetPostsCommand(
            PriceFrom,
            PriceTo,
            OfferPrice,
            Discount,
            LocId,
            CondType,
            PostType,
            SortType,
            ForPsn,
            CatId,
            BrandId,
            Page,
            PageSize
        ));

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById([FromRoute] int id)
    {
        var result = await mediator.Send(new GetPostByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("lite")]
    public async Task<IActionResult> GetLitePosts()
    {
        var result = await mediator.Send(new GetLitePostsQuery());
        return Ok(result);
    }
}
