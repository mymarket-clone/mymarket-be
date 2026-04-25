using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mymarket.Application.Features.Favorites.Commands.Add;
using Mymarket.Application.Features.Favorites.Commands.Remove;
using Mymarket.Application.Features.Favorites.Query;
using Mymarket.Application.Features.Posts.Commands.Add;
using Mymarket.Application.Features.Posts.Queries.Get;
using Mymarket.Application.Features.Posts.Queries.GetById;
using Mymarket.Application.Features.Posts.Queries.GetLite;
using Mymarket.Application.Features.Views.Commands.View;
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
        [FromQuery] SortType? SortBy,
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
            SortBy,
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

    [HttpGet("{id}/view")]
    public async Task<IActionResult> ViewPost([FromRoute] int id)
    {
        await mediator.Send(new ViewPostCommand(id));
        return NoContent();
    }

    [HttpGet("lite")]
    public async Task<IActionResult> GetLitePosts()
    {
        var result = await mediator.Send(new GetLitePostsQuery());
        return Ok(result);
    }

    [Authorize]
    [HttpGet("favorite")]
    public async Task<IActionResult> GetMyFavorites(
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 20)
    {
        var result = await mediator.Send(new GetFavoritesQuery(
            Page,
            PageSize
        ));
        return Ok(result);
    }

    [Authorize]
    [HttpPost("{id}/favorite")]
    public async Task<IActionResult> AddToFavorite([FromRoute] int id)
    {
        await mediator.Send(new AddToFavoriteCommand(id));
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}/favorite")]
    public async Task<IActionResult> RemoveFromFavorite([FromRoute] int id)
    {
        await mediator.Send(new RemoveFromFavouriteCommand(id));
        return NoContent();
    }
}
