using EFCoreSecondLevelCacheInterceptor;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Common;
using Mymarket.Application.Features.Posts.Models;
using Mymarket.Application.Features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Posts.Queries.GetById;

public record GetPostByIdQuery(int Id) : IRequest<PostDetailsDto?>;

public class GetPostByIdQueryHandler(
    IApplicationDbContext context,
    ILanguageContext languageContext,
    ICurrentUser currentUser
) : IRequestHandler<GetPostByIdQuery, PostDetailsDto?>
{
    public async Task<PostDetailsDto?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(p => new
            {
                Post = p,
                Images = p.PostsImages
                    .Where(pi => pi.Image != null && pi.Image.Url != null)
                    .OrderBy(pi => pi.Order)
                    .Select(pi => pi.Image!.Url!)
                    .ToList(),
                IsFavorite = currentUser.Id != 0 &&
                    p.Favorites.Any(f => f.UserId == currentUser.Id),
                ViewsCount = p.PostViews.Count(),
                PostAttributes = p.PostAttributes.ToList(),
                p.User,
                UserPostsCount = p.User!.Posts.Count()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (post == null) return null;

        var categories = await context.Categories
            .AsNoTracking()
            .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(10))
            .ToListAsync(cancellationToken);

        var categoryAttributes = await context.CategoryAttributes
            .AsNoTracking()
            .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(10))
            .Where(x => x.CategoryId == post.Post.CategoryId)
            .OrderBy(x => x.Order)
            .Include(x => x.Attribute)
            .ToListAsync(cancellationToken);

        var cityName = await context.Cities
            .AsNoTracking()
            .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(10))
            .Where(c => c.Id == post.Post.CityId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync(cancellationToken);

        var postAttrById = post.PostAttributes.ToDictionary(x => x.AttributeId);

        var selectedOptionIds = post.PostAttributes
            .Where(pa => pa.Value != null &&
                categoryAttributes.Any(ca =>
                    ca.AttributeId == pa.AttributeId &&
                    ca.Attribute?.AttributeType == AttributeType.Select))
            .Select(pa => int.Parse(pa.Value!))
            .ToList();

        var optionById = selectedOptionIds.Count > 0
            ? await context.AttributeOptions
                .AsNoTracking()
                .Where(x => selectedOptionIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken) : new Dictionary<int, AttributeOptionsEntity>();

        var attributesDto = new List<PostAttributeDto>();
        foreach (var categoryAttr in categoryAttributes)
        {
            postAttrById.TryGetValue(categoryAttr.AttributeId, out var postAttr);
            var attributeName = languageContext.LocalizeProperty<AttributeEntity>("Name")(categoryAttr.Attribute)!;
            string? value = null;

            if (postAttr != null)
            {
                if (categoryAttr.Attribute?.AttributeType == AttributeType.Select && postAttr.Value != null)
                {
                    var optionId = int.Parse(postAttr.Value);
                    if (optionById.TryGetValue(optionId, out var option))
                        value = languageContext.LocalizeProperty<AttributeOptionsEntity>("Name")(option);
                }
                else
                {
                    value = postAttr.Value;
                }
            }

            attributesDto.Add(new PostAttributeDto
            {
                Attribute = attributeName,
                Value = value,
                AttributeType = categoryAttr.Attribute?.AttributeType
            });
        }

        var u = post.User ?? throw new KeyNotFoundException(SharedResources.RecordNotFound);

        var userDto = new UserInfoDto(
            Id: u.Id,
            FirstName: u.Firstname,
            Lastname: u.LastName,
            Email: u.Email,
            GenderType: u.Gender,
            BirthYear: u.BirthYear,
            PhoneNumber: u.PhoneNumber,
            EmailVerified: u.EmailVerified,
            PostsCount: post.UserPostsCount
        );

        return new PostDetailsDto
        {
            Id = post.Post.Id,
            Title = languageContext.LocalizeProperty<PostEntity>("Title")(post.Post)!,
            Description = languageContext.LocalizeProperty<PostEntity>("Description")(post.Post),
            CategoryId = post.Post.CategoryId,
            Breadcrumb = BreadcrumbBuilder.Build(post.Post.CategoryId, categories, languageContext),
            Name = post.Post.Name,
            PhoneNumber = MaskPhoneNumber(post.Post.PhoneNumber),
            Price = post.Post.Price,
            PriceAfterDiscount = post.Post.SalePercentage > 0
                ? post.Post.Price * (1 - (double)post.Post.SalePercentage / 100)
                : null,
            CurrencyType = post.Post.CurrencyType,
            SalePercentage = post.Post.SalePercentage,
            CanOfferPrice = post.Post.CanOfferPrice,
            IsNegotiable = post.Post.IsNegotiable,
            ForDisabledPerson = post.Post.ForDisabledPerson,
            IsColored = post.Post.IsColored,
            AutoRenewal = post.Post.AutoRenewal,
            ConditionType = post.Post.ConditionType,
            PostType = post.Post.PostType,
            PromoType = post.Post.PromoType,
            Attributes = attributesDto,
            Images = post.Images,
            City = cityName,
            User = userDto,
            IsFavorite = post.IsFavorite,
            ViewsCount = post.ViewsCount,
            CreatedAt = post.Post.CreatedAt
        };
    }

    public static string MaskPhoneNumber(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var digits = new string(input.Where(char.IsDigit).ToArray());
        if (digits.Length < 5) return digits;

        var firstPart = digits.Substring(0, 3);
        var secondPart = digits.Substring(3, 2);

        return $"{firstPart} {secondPart} ** **";
    }
}