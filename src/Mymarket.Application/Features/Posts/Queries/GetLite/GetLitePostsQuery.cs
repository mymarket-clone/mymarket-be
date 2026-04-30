using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Posts.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;
using System.Linq.Dynamic.Core;

namespace Mymarket.Application.Features.Posts.Queries.GetLite;

public record GetLitePostsQuery : IRequest<PostLiteItemListDto>;


public class GetLitePostsQueryHandler(
    IApplicationDbContext context,
    ILanguageContext languageContext,
    ICurrentUser currentUser) : IRequestHandler<GetLitePostsQuery, PostLiteItemListDto>
{
    public async Task<PostLiteItemListDto> Handle(
        GetLitePostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await context.Posts
            .AsNoTracking()
            .Where(x => x.Status == PostStatus.Active)
            .Select(x => new
            {
                x.PromoType,
                Item = new PostLiteItemDto
                {
                    Id = x.Id,
                    Title = languageContext.LocalizeProperty<PostEntity>("Title")(x)!,
                    Price = x.Price,
                    IsNegotiable = x.IsNegotiable,
                    PriceAfterDiscount = x.SalePercentage > 0 ? x.Price * (1 - (double)x.SalePercentage / 100) : null,
                    CurrencyType = x.CurrencyType,
                    Images = x.PostsImages
                        .Where(pi => pi.Image != null && pi.Image.Url != null)
                        .OrderBy(pi => pi.Order)
                        .Select(pi => pi.Image!.Url!)
                        .ToList(),
                    IsFavorite = context.Favorites.Any(f => f.PostId == x.Id && f.UserId == currentUser.Id)
                }
            })
            .ToListAsync(cancellationToken);

        return new PostLiteItemListDto
        {
            SuperVip = [.. posts
                .Where(x => x.PromoType == PromoType.SUPER_VIP)
                .Select(x => x.Item)],

            VipPlus = [.. posts
                .Where(x => x.PromoType == PromoType.VIP_PLUS)
                .Select(x => x.Item)],

            Vip = [.. posts
                .Where(x => x.PromoType == PromoType.VIP)
                .Select(x => x.Item)]
        };
    }
}