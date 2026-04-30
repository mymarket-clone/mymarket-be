using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Common.Exceptions;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;
using System.Globalization;
using System.Text.Json;

namespace Mymarket.Application.Features.Posts.Commands.Add;

public record AddPostCommand(
    PostType PostType,
    int CategoryId,
    string? YoutubeLink,
    ConditionType ConditionType,
    List<IFormFile> Images,
    int? BrandId,
    IFormFile MainImage,
    string Title,
    string? Description,
    string? TitleEn,
    string? DescriptionEn,
    string? TitleRu,
    string? DescriptionRu,
    bool ForDisabledPerson,
    double Price,
    CurrencyType CurrencyType,
    byte SalePercentage,
    bool CanOfferPrice,
    bool IsNegotiable,
    int CityId,
    string Name,
    string PhoneNumber,
    PromoType? PromoType,
    int? PromoDays,
    bool IsColored,
    int? ColorDays,
    bool AutoRenewal,
    int? AutoRenewalOnceIn,
    int? AutoRenewalAtTime,
    string? AttributesJson
) : IRequest<Unit>;

public sealed class AddPostCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser,
    IImageService imageService) : IRequestHandler<AddPostCommand, Unit>
{
    public async Task<Unit> Handle(AddPostCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        var imagesToUpload = request.Images?.Where(image => image.Length > 0).ToList() ?? [];

        if (imagesToUpload.Count == 0 && request.MainImage is { Length: > 0 })
        {
            imagesToUpload.Add(request.MainImage);
        }

        var uploadedImages = await imageService.UploadAsync(imagesToUpload, cancellationToken);

        try
        {
            if (currentUser.Id is null)
            {
                throw new UnauthorizedAccessException("No user found");
            }

            List<AttributeItem> attributes;
            try
            {
                attributes = JsonSerializer.Deserialize<List<AttributeItem>>(request.AttributesJson ?? "[]") ?? [];
            }
            catch
            {
                throw new AttributeValidationException(new Dictionary<string, string[]>
                {
                    ["attributesJson"] = ["Invalid JSON format"]
                });
            }

            var errors = new Dictionary<string, List<string>>();

            var attributeIds = attributes.Select(a => a.Id).Distinct().ToList();

            var dbAttributes = await context.Attributes
                .Where(a => attributeIds.Contains(a.Id))
                .Select(a => new { a.Id, a.AttributeType })
                .ToListAsync(cancellationToken);

            var attributeTypeById = dbAttributes.ToDictionary(x => x.Id, x => x.AttributeType);

            var selectAttributeIds = dbAttributes
                .Where(x => x.AttributeType == AttributeType.Select)
                .Select(x => x.Id)
                .ToList();

            var optionIdsByAttributeId = await context.AttributeOptions
                .Where(o => selectAttributeIds.Contains(o.AttributeId))
                .GroupBy(o => o.AttributeId)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Select(o => o.Id).ToHashSet(),
                    cancellationToken
                );

            var attributeMeta = await context.CategoryAttributes
                .Where(ca => ca.CategoryId == request.CategoryId)
                .Select(ca => new
                {
                    Id = ca.AttributeId,
                    Type = ca.Attribute!.AttributeType,
                    ca.IsRequired
                })
                .ToListAsync(cancellationToken);

            var metaById = attributeMeta.ToDictionary(x => x.Id);

            var missingInCategory = attributeIds.Where(id => !metaById.ContainsKey(id)).ToList();

            foreach (var id in missingInCategory)
                AddError(errors, id.ToString(), "Attribute is not allowed for this category.");

            var attributesById = attributes
                .GroupBy(a => a.Id)
                .ToDictionary(g => g.Key, g => g.First());

            var missingRequired = attributeMeta
                .Where(meta => meta.IsRequired &&
                    (!attributesById.TryGetValue(meta.Id, out var attr) || IsEmpty(attr.Value)))
                .Select(meta => meta.Id)
                .ToList();

            foreach (var id in missingRequired)
                AddError(errors, id.ToString(), "Value is required.");

            foreach (var attr in attributes)
            {
                var key = attr.Id.ToString();

                if (!metaById.TryGetValue(attr.Id, out var meta))
                {
                    continue;
                }

                if (!attributeTypeById.TryGetValue(attr.Id, out var type))
                {
                    AddError(errors, key, "Unknown attribute id");
                    continue;
                }

                switch (type)
                {
                    case AttributeType.Text:
                        {
                            if (attr.Value.ValueKind != JsonValueKind.String)
                            {
                                AddError(errors, key, "Value must be a string");
                                break;
                            }

                            var s = attr.Value.GetString();
                            if (string.IsNullOrWhiteSpace(s))
                                AddError(errors, key, "Text cannot be empty");

                            break;
                        }

                    case AttributeType.Number:
                        {
                            if (attr.Value.ValueKind == JsonValueKind.Number)
                            {
                                break;
                            }

                            if (attr.Value.ValueKind == JsonValueKind.String)
                            {
                                var s = attr.Value.GetString();
                                if (!decimal.TryParse(s, out _))
                                    AddError(errors, key, "Value must be a number");
                                break;
                            }

                            AddError(errors, key, "Value must be a number");
                            break;
                        }

                    case AttributeType.Select:
                        {
                            var isEmpty =
                                attr.Value.ValueKind == JsonValueKind.Null ||
                                (attr.Value.ValueKind == JsonValueKind.String && string.IsNullOrWhiteSpace(attr.Value.GetString()));

                            if (isEmpty)
                            {
                                if (meta.IsRequired)
                                    AddError(errors, key, "Value is required.");
                                break;
                            }

                            var optionId = ExtractInt(attr.Value);

                            if (!optionIdsByAttributeId.TryGetValue(attr.Id, out var validOptions) || !validOptions.Contains(optionId))
                                AddError(errors, key, "Value must be a valid option id.");

                            break;
                        }

                    case AttributeType.Bool:
                        {
                            int b;
                            if (attr.Value.ValueKind == JsonValueKind.Number && attr.Value.TryGetInt32(out var n))
                                b = n;
                            else if (attr.Value.ValueKind == JsonValueKind.String && int.TryParse(attr.Value.GetString(), out var s))
                                b = s;
                            else
                            {
                                AddError(errors, key, "Invalid value");
                                break;
                            }

                            if (b != 1 && b != 2)
                                AddError(errors, key, "Invalid value");

                            break;
                        }

                    default:
                        AddError(errors, key, "Unsupported attribute type");
                        break;
                }
            }

            if (errors.Count != 0)
            {
                throw new AttributeValidationException(
                    errors.ToDictionary(k => k.Key, v => v.Value.ToArray())
                );
            }

            var post = new PostEntity
            {
                PostType = request.PostType,
                CategoryId = request.CategoryId,
                YoutubeLink = request.YoutubeLink,
                ConditionType = request.ConditionType,
                Title = request.Title,
                Description = request.Description,
                TitleEn = request.TitleEn,
                DescriptionEn = request.DescriptionEn,
                TitleRu = request.TitleRu,
                DescriptionRu = request.DescriptionRu,
                ForDisabledPerson = request.ForDisabledPerson,
                Price = request.Price,
                CurrencyType = request.CurrencyType,
                SalePercentage = request.SalePercentage,
                CanOfferPrice = request.CanOfferPrice,
                IsNegotiable = request.IsNegotiable,
                CityId = request.CityId,
                Name = request.Name,
                BrandId = request.BrandId,
                PhoneNumber = request.PhoneNumber,
                UserId = (int)currentUser.Id,
                PromoType = request.PromoType,
                PromoDays = request.PromoDays,
                IsColored = request.IsColored,
                ColorDays = request.ColorDays,
                AutoRenewal = request.AutoRenewal,
                AutoRenewalOnceIn = request.AutoRenewalOnceIn,
                AutoRenewalAtTime = request.AutoRenewalAtTime
            };

            await context.Posts.AddAsync(post, cancellationToken);

            for (int i = 0; i < uploadedImages.Count; i++)
            {
                post.PostsImages.Add(new PostsImagesEntity
                {
                    Post = post,
                    Image = uploadedImages[i],
                    Order = i + 1
                });
            }

            await context.SaveChangesAsync(cancellationToken);

            var postAttributes = attributes
                .Where(a => metaById.ContainsKey(a.Id))
                .Select(a => new { a, meta = metaById[a.Id] })
                .Where(x => !IsEmpty(x.a.Value))
                .Select(x => new PostAttributesEntity
                {
                    PostId = post.Id,
                    AttributeId = x.a.Id,
                    ValueType = x.meta.Type,
                    Value = NormalizeAttributeValue(x.meta.Type, x.a.Value)
                })
                .ToList();


            context.PostAttributes.AddRange(postAttributes);

            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AddPostCommand failed: {ex}");
            await transaction.RollbackAsync(cancellationToken);
            await imageService.DeleteAsync(uploadedImages, cancellationToken);
            throw;
        }
    }

    private static void AddError(
        Dictionary<string, List<string>> errors,
        string key,
        string message)
    {
        if (!errors.TryGetValue(key, out var list))
        {
            list = new List<string>();
            errors[key] = list;
        }

        list.Add(message);
    }

    static int ExtractInt(JsonElement el)
    {
        if (el.ValueKind == JsonValueKind.Number && el.TryGetInt32(out var n)) return n;
        if (el.ValueKind == JsonValueKind.String && int.TryParse(el.GetString(), out var s)) return s;
        throw new FormatException("Expected int value.");
    }

    static string NormalizeAttributeValue(AttributeType type, JsonElement value)
    {
        return type switch
        {
            AttributeType.Text => value.ValueKind == JsonValueKind.String
                                        ? value.GetString()!.Trim()
                                        : throw new InvalidOperationException("Text attribute must be a string"),

            AttributeType.Number => value.ValueKind switch
            {
                JsonValueKind.Number => value.GetDecimal().ToString(CultureInfo.InvariantCulture),
                JsonValueKind.String => value.GetString()!.Trim(),
                _ => throw new InvalidOperationException("Number attribute must be a number or numeric string")
            },

            AttributeType.Select => ExtractInt(value).ToString(CultureInfo.InvariantCulture),

            AttributeType.Bool => ExtractInt(value).ToString(CultureInfo.InvariantCulture),

            _ => throw new InvalidOperationException($"Unsupported attribute type: {type}")
        };
    }

    static bool IsEmpty(JsonElement v)
    {
        return v.ValueKind == JsonValueKind.Null || (v.ValueKind == JsonValueKind.String && string.IsNullOrWhiteSpace(v.GetString()));
    }
}
