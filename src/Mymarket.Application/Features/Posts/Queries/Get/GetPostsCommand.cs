using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCoreSecondLevelCacheInterceptor;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Common;
using Mymarket.Application.Features.Posts.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Common;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Posts.Queries.Get;

public record GetPostsCommand(
    int? PriceFrom,
    int? PriceTo,
    bool? OfferPrice,
    bool? Discount,
    int? LocId,
    List<ConditionType>? CondType,
    List<PostType>? PostType,
    SortType? SortType,
    bool? ForPsn,
    int? CatId,
    int? BrandId,
    SortType? SortBy,
    int Page,
    int PageSize
) : IRequest<PostSearchDto>;


public class GetPostsCommandHandler(
    IApplicationDbContext context,
    ILanguageContext languageContext,
    ICurrentUser currentUser
) : IRequestHandler<GetPostsCommand, PostSearchDto>
{
    public async Task<PostSearchDto> Handle(GetPostsCommand request, CancellationToken cancellationToken)
    {
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var categories = await context.Categories
            .AsNoTracking()
            .Include(x => x.Logo)
            .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(10))
            .ToListAsync(cancellationToken);

        var childrenLookup = categories
            .GroupBy(c => c.ParentId ?? 0)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Id).ToList());

        List<int> GetAllDescendants(int categoryId)
        {
            var result = new List<int> { categoryId };
            if (!childrenLookup.TryGetValue(categoryId, out var children))
                return result;
            foreach (var child in children)
                result.AddRange(GetAllDescendants(child));  
            return result;
        }

        var query = ApplyFilters(context.Posts.AsNoTracking().AsQueryable(), request);

        if (request.CatId is not null)
        {
            var categoryIds = GetAllDescendants(request.CatId.Value);
            query = query.Where(p => categoryIds.Contains(p.CategoryId));
        }

        query = request.SortType switch
        {
            SortType.DateDesc => query
                .OrderBy(p =>
                    p.PromoType == PromoType.SUPER_VIP ? 0 :
                    p.PromoType == PromoType.VIP_PLUS ? 1 :
                    p.PromoType == PromoType.VIP ? 2 : 3)
                .ThenByDescending(p => p.CreatedAt),

            SortType.DateAsc => query
                .OrderBy(p => p.CreatedAt),

            SortType.PriceDesc => query
                .OrderBy(p => p.IsNegotiable ? 1 : 0)
                .ThenByDescending(p =>
                    p.IsNegotiable ? null : p.Price * (1 - p.SalePercentage / 100.0)
                ),

            SortType.PriceAsc => query
                .OrderBy(p => p.IsNegotiable ? 0 : 1)
                .ThenBy(p =>
                    p.IsNegotiable ? null : p.Price * (1 - p.SalePercentage / 100.0)
                ),

            SortType.WithDiscount => query
                .OrderByDescending(p => p.SalePercentage)
                .ThenByDescending(p => p.CreatedAt),


            SortType.ViewsDesc => query
                .OrderByDescending(p => p.PostViews.Count())
                .ThenByDescending(p => p.CreatedAt),
                
            SortType.ViewsAsc => query
                .OrderBy(p => p.PostViews.Count())
                .ThenBy(p => p.CreatedAt),

            _ => query
                .OrderBy(p =>
                    p.PromoType == PromoType.SUPER_VIP ? 0 :
                    p.PromoType == PromoType.VIP_PLUS ? 1 :
                    p.PromoType == PromoType.VIP ? 2 : 3)
                .ThenByDescending(p => p.CreatedAt)
        };

        var skip = (request.Page - 1) * pageSize;

        var postsWithImages = await query
            .Skip(skip)
            .Take(pageSize)
            .Select(p => new
            {
                Post = p,
                Images = p.PostsImages
                    .Where(pi => pi.Image != null && pi.Image.Url != null)
                    .OrderBy(pi => pi.Order)
                    .Select(pi => pi.Image!.Url!)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        var totalCount = await query.CountAsync(cancellationToken);

        var postIds = postsWithImages.Select(x => x.Post.Id).ToList();

        var favoritePostIds = currentUser.Id != 0
            ? (await context.Favorites
                .AsNoTracking()
                .Where(f => f.UserId == currentUser.Id && postIds.Contains(f.PostId))
                .Select(f => f.PostId)
                .ToListAsync(cancellationToken))
                .ToHashSet()
            : new HashSet<int>();

        List<int> scopeCategoryIds = request.CatId is not null
            ? GetAllDescendants(request.CatId.Value)
            : categories.Where(c => c.ParentId == null)
                        .SelectMany(c => GetAllDescendants(c.Id))
                        .ToList();

        var postCounts = await context.Posts
            .AsNoTracking()
            .Where(p => scopeCategoryIds.Contains(p.CategoryId))
            .GroupBy(p => p.CategoryId)
            .Select(g => new { CategoryId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Count, cancellationToken);

        var items = postsWithImages.Select(x =>
        {
            var p = x.Post;
            return new PostDto
            {
                Id = p.Id,
                AutoRenewal = p.AutoRenewal,
                CanOfferPrice = p.CanOfferPrice,
                CategoryId = p.CategoryId,
                ConditionType = p.ConditionType,
                CurrencyType = p.CurrencyType,
                Description = languageContext.LocalizeProperty<PostEntity>("Description")(p),
                ForDisabledPerson = p.ForDisabledPerson,
                IsColored = p.IsColored,
                IsNegotiable = p.IsNegotiable,
                Name = p.Name,
                PhoneNumber = p.PhoneNumber,
                PostType = p.PostType,
                Price = p.Price,
                PromoType = p.PromoType,
                SalePercentage = p.SalePercentage,
                Title = languageContext.LocalizeProperty<PostEntity>("Title")(p)!,
                BrandId = p.BrandId,
                PriceAfterDiscount = p.Price.HasValue
                    ? p.Price.Value * (1 - p.SalePercentage / 100.0)
                    : null,
                Images = x.Images,
                IsFavorite = favoritePostIds.Contains(p.Id),
            };
        }).ToList();

        List<CategoryEntity> categoryScope;
        if (request.CatId is null)
        {
            categoryScope = [.. categories.Where(c => c.ParentId == null)];
        }
        else
        {
            var selected = categories.FirstOrDefault(c => c.Id == request.CatId);
            if (selected is null)
            {
                categoryScope = [.. categories.Where(c => c.ParentId == null)];
            }
            else
            {
                var children = categories.Where(c => c.ParentId == selected.Id).ToList();
                if (children.Count != 0)
                    categoryScope = children;
                else if (selected.ParentId is int parentId)
                    categoryScope = [.. categories.Where(c => c.ParentId == parentId)];
                else
                    categoryScope = [.. categories.Where(c => c.ParentId == null)];
            }
        }

        var categoryDtos = categoryScope
            .Select(c =>
            {
                var allIds = GetAllDescendants(c.Id);
                var total = allIds.Sum(id => postCounts.TryGetValue(id, out var count) ? count : 0);
                return new CategoryLiteDto
                {
                    Id = c.Id,
                    Name = languageContext.LocalizeProperty<CategoryEntity>("Name")(c) ?? string.Empty,
                    Count = total,
                    HasChildren = childrenLookup.ContainsKey(c.Id)
                };
            })
            .ToList();

        List<CategoryBreadcrumbDto>? breadcrumb = request.CatId switch
        {
            int id when id > 0 => BreadcrumbBuilder.Build(id, categories, languageContext),
            _ => null
        };

        return new PostSearchDto
        {
            Result = new PaginatedResult<PostDto>
            {
                Items = items,
                Page = request.Page,
                PageSize = pageSize,
                TotalCount = totalCount
            },
            Breadcrumb = breadcrumb?.Count > 0 ? breadcrumb : null,
            Categories = categoryDtos,
        };
    }

    public static IQueryable<PostEntity> ApplyFilters(IQueryable<PostEntity> query, GetPostsCommand request)
    {
        if (request.PriceFrom is not null)
        {
            query = query.Where(p =>
                p.Price * (1 - p.SalePercentage / 100.0) >= request.PriceFrom);
        }

        if (request.PriceTo is not null)
        {
            query = query.Where(p =>
                p.Price * (1 - p.SalePercentage / 100.0) <= request.PriceTo);
        }

        if (request.OfferPrice is true)
        {
            query = query.Where(p => !p.CanOfferPrice);
        }

        if (request.Discount is true)
        {
            query = query.Where(p => p.SalePercentage > 0);
        }

        if (request.LocId is not null)
        {
            query = query.Where(p => p.CityId == request.LocId);
        }

        if (request.CondType is { Count: > 0 })
        {
            query = query.Where(p => request.CondType.Contains(p.ConditionType));
        }

        if (request.PostType is { Count: > 0 })
        {
            query = query.Where(p => request.PostType.Contains(p.PostType));
        }

        if (request.BrandId is not null)
        {
            query = query.Where(p => p.BrandId == request.BrandId);
        }

        if (request.ForPsn is not null)
        {
            query = query.Where(p => p.ForDisabledPerson == request.ForPsn);
        }

        return query;
    }
}