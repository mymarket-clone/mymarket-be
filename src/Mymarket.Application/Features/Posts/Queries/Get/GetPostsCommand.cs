using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Common;
using Mymarket.Application.Features.Categories.Models;
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
    int? CatId,
    int? BrandId,
    int Page,
    int PageSize
) : IRequest<PostSearchDto>;

public class GetPostsCommandHandler(
    IApplicationDbContext context,
    IMapper mapper, 
    ILanguageContext languageContext) : IRequestHandler<GetPostsCommand, PostSearchDto>
{
    public async Task<PostSearchDto> Handle(GetPostsCommand request, CancellationToken cancellationToken)
    {
        var query = ApplyFilters(context.Posts.AsQueryable(), request);

        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var categories = await context.Categories
            .AsNoTracking()
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
            {
                result.AddRange(GetAllDescendants(child));
            }

            return result;
        }

        if (request.CatId is not null)
        {
            var categoryIds = GetAllDescendants(request.CatId.Value);
            query = query.Where(p => categoryIds.Contains(p.CategoryId));
        }

        var postCounts = await context.Posts
            .GroupBy(p => p.CategoryId)
            .Select(g => new { CategoryId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CategoryId, x => x.Count, cancellationToken);

        var ordered = query
            .OrderByDescending(p => p.PromoType)
            .ThenByDescending(p => p.CreatedAt);

        query = request.SortType switch
        {
            SortType.DateDec => ordered.ThenByDescending(p => p.CreatedAt),
            SortType.DateAsc => ordered.ThenBy(p => p.CreatedAt),
            SortType.PriceDec => ordered.ThenByDescending(p => p.Price - (p.Price * p.SalePercentage / 100)),
            SortType.PriceAsc => ordered.ThenBy(p => p.Price - (p.Price * p.SalePercentage / 100)),
            _ => ordered
        };

        var skip = (request.Page - 1) * pageSize;

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip(skip)
            .Take(pageSize)
            .ProjectTo<PostDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var postIds = items.Select(x => x.Id).ToList();

        var images = await context.PostsImages
            .AsNoTracking()
            .Where(pi =>
                postIds.Contains(pi.PostId) &&
                pi.Image != null &&
                pi.Image.Url != null)
            .OrderBy(pi => pi.Order)
            .Select(pi => new
            {
                pi.PostId,
                Url = pi.Image!.Url!
            })
            .ToListAsync(cancellationToken);


        var imageLookup = images
            .GroupBy(x => x.PostId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Url).ToList()
            );

        List<CategoryEntity> categoryScope;

        if (request.CatId is null)
        {
            categoryScope = categories
                .Where(c => c.ParentId == null)
                .ToList();
        }
        else
        {
            var selected = categories.FirstOrDefault(c => c.Id == request.CatId);

            if (selected is null)
            {
                categoryScope = categories
                    .Where(c => c.ParentId == null)
                    .ToList();
            }
            else
            {
                var children = categories
                    .Where(c => c.ParentId == selected.Id)
                    .ToList();

                if (children.Count != 0)
                {
                    categoryScope = children;
                }
                else
                {
                    if (selected.ParentId is int parentId)
                    {
                        categoryScope = categories
                            .Where(c => c.ParentId == parentId)
                            .ToList();
                    }
                    else
                    {
                        categoryScope = categories
                            .Where(c => c.ParentId == null)
                            .ToList();
                    }
                }
            }
        }

        var categoryDtos = categoryScope
            .Select(c =>
            {
                var allIds = GetAllDescendants(c.Id);

                var totalCount = allIds
                    .Sum(id => postCounts.TryGetValue(id, out var count) ? count : 0);

                return new CategoryLiteDto
                {
                    Id = c.Id,
                    Name = languageContext.LocalizeProperty<CategoryEntity>("Name")(c) ?? string.Empty,
                    Count = totalCount,
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
                p.Price - (p.Price * p.SalePercentage / 100) >= request.PriceFrom);
        }

        if (request.PriceTo is not null)
        {
            query = query.Where(p =>
                p.Price - (p.Price * p.SalePercentage / 100) <= request.PriceTo);
        }

        if (request.OfferPrice is not null && request.OfferPrice.Value == false)
        {
            query = query.Where(p => !p.CanOfferPrice);
        }

        if (request.Discount is not null && request.Discount.Value)
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

        return query;
    }
}
