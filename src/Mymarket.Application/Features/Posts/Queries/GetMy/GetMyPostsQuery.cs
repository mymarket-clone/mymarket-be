using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Posts.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Common;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Posts.Queries.GetMy;

public record GetMyPostsQuery(
    int Page,
    int PageSize,
    PostStatus Status
) : IRequest<PostMyDto>;

public class GetMyPostsQueryHandler(
    IApplicationDbContext context,
    ILanguageContext languageContext,
    ICurrentUser currentUser) : IRequestHandler<GetMyPostsQuery, PostMyDto>
{
    public async Task<PostMyDto> Handle(GetMyPostsQuery request, CancellationToken cancellationToken)
    {
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var skip = (request.Page - 1) * pageSize;

        var query = context.Posts
            .Where(x => x.UserId == currentUser.Id && x.Status == request.Status);

        var totalCount = await query.CountAsync(cancellationToken);

        var posts = await query
            .Skip(skip)
            .Take(pageSize)
            .Select(x => new PostMyItemDto
            {
                Id = x.Id,
                Title = languageContext.LocalizeProperty<PostEntity>("Title")(x)!,
                Price = (double)x.Price!,
                ImageUrl = x.PostsImages!
                    .Select(pi => pi.Image!.Url)
                    .FirstOrDefault()!,
                CreatedAt = x.CreatedAt,
                Status = x.Status,
                ViewsCount = x.PostViews.Count(),
                CurrencyType = x.CurrencyType,
                IsNegotiable = x.IsNegotiable,
                PriceAfterDiscount = x.Price.HasValue
                    ? x.Price.Value * (1 - x.SalePercentage / 100.0)
                    : null,
            })
            .ToListAsync(cancellationToken);

        var stats = await context.Posts
            .Where(x => x.UserId == currentUser.Id)
            .GroupBy(x => x.UserId)
            .Select(g => new
            {
                Active = g.Count(x => x.Status == PostStatus.Active),
                Inactive = g.Count(x => x.Status == PostStatus.Inactive),
                Blocked = g.Count(x => x.Status == PostStatus.Blocked),
                Views = g.Sum(x => x.PostViews.Count)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return new PostMyDto
        {
            Result = new PaginatedResult<PostMyItemDto>
            {
                Items = posts,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = pageSize,
            },
            ActiveCount = stats?.Active ?? 0,
            InactiveCount = stats?.Inactive ?? 0,
            BlockedCount = stats?.Blocked ?? 0,
            TotalViews = stats?.Views ?? 0
        };
    }
}
