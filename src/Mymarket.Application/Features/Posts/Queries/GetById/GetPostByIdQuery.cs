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
    ILanguageContext languageContext
) : IRequestHandler<GetPostByIdQuery, PostDetailsDto?>
{
    public async Task<PostDetailsDto?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await context.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (post == null)  return null;

        var categories = await context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var categoryAttributes = await context.CategoryAttributes
            .AsNoTracking()
            .Where(x => x.CategoryId == post.CategoryId)
            .OrderBy(x => x.Order)
            .Include(x => x.Attribute)
            .ToListAsync(cancellationToken);

        var postCity = await context.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == post.CityId, cancellationToken);

        var postAttributes = await context.PostAttributes
            .AsNoTracking()
            .Where(x => x.PostId == post.Id)
            .ToListAsync(cancellationToken);

        var postImages = await context.PostsImages
            .AsNoTracking()
            .Where(pi => pi.PostId == request.Id && pi.Image != null && pi.Image.Url != null)
            .OrderBy(pi => pi.Order)
            .Select(pi => pi.Image!.Url!)
            .ToListAsync(cancellationToken);

        var postAttrById = postAttributes.ToDictionary(x => x.AttributeId);

        var selectedOptionIds = postAttributes
            .Where(pa => pa.Value != null &&
                    categoryAttributes.Any(ca => ca.AttributeId == pa.AttributeId &&
                    ca.Attribute?.AttributeType == AttributeType.Select))
            .Select(pa => int.Parse(pa.Value!))
            .ToList();

        var optionById = await context.AttributeOptions
            .AsNoTracking()
            .Where(x => selectedOptionIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        var attributesDto = new List<PostAttributeDto>();

        foreach (var categoryAttr in categoryAttributes)
        {
            postAttrById.TryGetValue(categoryAttr.AttributeId, out var postAttr);

            var attributeName = languageContext.LocalizeProperty<AttributeEntity>("Name")(categoryAttr.Attribute)!;
            string? value = null;

            var attributeType = categoryAttr.Attribute?.AttributeType;

            if (postAttr != null)
            {
                if (attributeType == AttributeType.Select && postAttr.Value != null)
                {
                    var optionId = int.Parse(postAttr.Value);
                    if (optionById.TryGetValue(optionId, out var option))
                    {
                        value = languageContext.LocalizeProperty<AttributeOptionsEntity>("Name")(option);
                    }
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
                AttributeType = attributeType
            });
        }

        var user = context.Users.FirstOrDefault(u => u.Id == post.UserId)
            ?? throw new KeyNotFoundException(SharedResources.RecordNotFound);

        var userDto = new UserInfoDto(
            Id: user.Id,
            FirstName: user.Firstname,
            Lastname: user.LastName,
            Email: user.Email,
            GenderType: user.Gender,
            BirthYear: user.BirthYear,
            PhoneNumber: user.PhoneNumber,
            EmailVerified: user.EmailVerified,
            PostsCount: context.Posts.Count(p => p.UserId == user.Id)
        );

        var postDto = new PostDetailsDto
        {
            Id = post.Id,
            Title = languageContext.LocalizeProperty<PostEntity>("Title")(post)!,
            Description = languageContext.LocalizeProperty<PostEntity>("Description")(post),
            CategoryId = post.CategoryId,
            Breadcrumb = BreadcrumbBuilder.Build(post.CategoryId, categories, languageContext),
            Name = post.Name,
            PhoneNumber = MaskPhoneNumber(post.PhoneNumber),
            Price = post.Price,
            PriceAfterDiscount = post.SalePercentage > 0 ? post.Price * (1 - (double)post.SalePercentage / 100) : null,
            CurrencyType = post.CurrencyType,
            SalePercentage = post.SalePercentage,
            CanOfferPrice = post.CanOfferPrice,
            IsNegotiable = post.IsNegotiable,
            ForDisabledPerson = post.ForDisabledPerson,
            IsColored = post.IsColored,
            AutoRenewal = post.AutoRenewal,
            ConditionType = post.ConditionType,
            PostType = post.PostType,
            PromoType = post.PromoType,
            Attributes = attributesDto,
            Images = postImages,
            City = postCity?.Name ?? null,
            User = userDto
        };

        return postDto;
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