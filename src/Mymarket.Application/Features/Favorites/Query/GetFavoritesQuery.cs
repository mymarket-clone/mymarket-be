using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Posts.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Common;

namespace Mymarket.Application.Features.Favorites.Query;

public record GetFavoritesQuery(
    int Page,
    int PageSize
) : IRequest<PaginatedResult<PostDto>>;

public class GetFavoritesQueryHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<GetFavoritesQuery, PaginatedResult<PostDto>>
{
    public async Task<PaginatedResult<PostDto>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
    {
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var skip = (request.Page - 1) * pageSize;

        var baseQuery = context.Favorites
            .AsNoTracking()
            .Where(f => f.UserId == currentUser.Id)
            .Select(f => f.Post!)
            .Where(p => p != null);

        var totalCount = baseQuery.Count();

        var items = await baseQuery
            .OrderByDescending(p => p.PromoType)
            .ThenByDescending(p => p.CreatedAt)
            .Skip(skip)
            .Take(pageSize)
            .Select(p => new PostDto
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                IsNegotiable = p.IsNegotiable,
                PriceAfterDiscount = p.SalePercentage > 0
                    ? p.Price * (1 - (double)p.SalePercentage / 100)
                    : null,
                CurrencyType = p.CurrencyType,
                Images = p.PostsImages
                    .Where(pi => pi.Image != null && pi.Image.Url != null)
                    .OrderBy(pi => pi.Order)
                    .Select(pi => pi.Image!.Url!)
                    .ToList(),
                IsFavorite = true
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<PostDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
