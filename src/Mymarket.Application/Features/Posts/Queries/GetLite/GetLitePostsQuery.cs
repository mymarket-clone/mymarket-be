using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Posts.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Posts.Queries.GetLite;

public record GetLitePostsQuery : IRequest<PostLiteItemListDto>;


public class GetLitePostsQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetLitePostsQuery, PostLiteItemListDto>
{
    public async Task<PostLiteItemListDto> Handle(
        GetLitePostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await context.Posts
            .AsNoTracking()
            .Select(x => new
            {
                x.PromoType,
                Item = new PostLiteItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    CurrencyType = x.CurrencyType,
                    Images = x.PostsImages
                        .Where(pi => pi.Image != null && pi.Image.Url != null)
                        .OrderBy(pi => pi.Order)
                        .Select(pi => pi.Image!.Url!)
                        .ToList()
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