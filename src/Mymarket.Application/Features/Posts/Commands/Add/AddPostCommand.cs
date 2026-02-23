using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mymarket.Application.Features.Posts.Commands.Add;

public record AddPostCommand(
    PostType PostType,
    int CategoryId,
    string? YoutubeLink,
    ConditionType? ConditionType,
    List<IFormFile> Images,
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
    int UserId,
    PromoType? PromoType,
    int? PromoDays,
    bool IsColored,
    int? ColorDays,
    bool AutoRenewal,
    int? AutoRenewalOnceIn,
    int? AutoRenewalAtTime,
    string? AttributesJson
) : IRequest<Unit>;

public record AttributeItem(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("value")] JsonElement Value
);

public class AttributeValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }
    public AttributeValidationException(Dictionary<string, string[]> errors) => Errors = errors;
}

public sealed class ApiValidationProblem : ProblemDetails
{
    public Dictionary<string, string[]> Errors { get; init; } = [];
    public string Code { get; init; } = "ValidationError";
}

public sealed class AddPostCommandHandler(
    IApplicationDbContext _context,
    IImageService _image) : IRequestHandler<AddPostCommand, Unit>
{
    public async Task<Unit> Handle(AddPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            List<AttributeItem> attributes;
            try
            {
                attributes = JsonSerializer.Deserialize<List<AttributeItem>>(request.AttributesJson ?? "[]") ?? [];
            }
            catch
            {
                throw new AttributeValidationException(new Dictionary<string, string[]>
                {
                    ["attributesJson"] = ["Invalid JSON format."]
                });
            }

            var errors = new Dictionary<string, List<string>>();

            if (attributes.Count == 0)
            {
                AddError(errors, "attributesJson", "Attributes array cannot be empty.");
            }

            var attributeIds = attributes.Select(a => a.Id).Distinct().ToList();

            var dbAttributes = await _context.Attributes
                .Where(a => attributeIds.Contains(a.Id))
                .Select(a => new { a.Id, a.AttributeType ,  })
                .ToListAsync(cancellationToken);

            var attributeTypeById = dbAttributes.ToDictionary(x => x.Id, x => x.AttributeType);

            var selectAttributeIds = dbAttributes
                .Where(x => x.AttributeType == AttributeType.Select)
                .Select(x => x.Id)
                .ToList();

            var optionIdsByAttributeId = await _context.AttributesOptions
                .Where(o => selectAttributeIds.Contains(o.AttributeId))
                .GroupBy(o => o.AttributeId)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Select(o => o.Id).ToHashSet(),
                    cancellationToken
                );

            foreach (var attr in attributes)
            {
                var key = attr.Id.ToString();

                if (!attributeTypeById.TryGetValue(attr.Id, out var type))
                {
                    AddError(errors, key, "Unknown attribute id.");
                    continue;
                }

                switch (type)
                {
                    case AttributeType.Text:
                        {
                            if (attr.Value.ValueKind != JsonValueKind.String)
                            {
                                AddError(errors, key, "Value must be a string.");
                                break;
                            }

                            var s = attr.Value.GetString();
                            if (string.IsNullOrWhiteSpace(s))
                                AddError(errors, key, "Text cannot be empty.");

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
                                    AddError(errors, key, "Value must be a number.");
                                break;
                            }

                            AddError(errors, key, "Value must be a number.");
                            break;
                        }

                    case AttributeType.Select:
                        {
                            int optionId;
                            if (attr.Value.ValueKind == JsonValueKind.Number && attr.Value.TryGetInt32(out var n))
                            {
                                optionId = n;
                            }
                            else if (attr.Value.ValueKind == JsonValueKind.String && int.TryParse(attr.Value.GetString(), out var s))
                            {
                                optionId = s;
                            }
                            else
                            {
                                AddError(errors, key, "Value must be an option id.");
                                break;
                            }

                            if (!optionIdsByAttributeId.TryGetValue(attr.Id, out var validOptions) || !validOptions.Contains(optionId))
                                AddError(errors, key, "Value must be a valid option id.");

                            break;
                        }

                    case AttributeType.Bool:
                        {
                            int b;
                            if (attr.Value.ValueKind == JsonValueKind.Number && attr.Value.TryGetInt32(out var n))
                            {
                                b = n;
                            }
                            else if (attr.Value.ValueKind == JsonValueKind.String && int.TryParse(attr.Value.GetString(), out var s))
                            {
                                b = s;
                            }
                            else
                            {
                                AddError(errors, key, "Value must be 1 (false) or 2 (true).");
                                break;
                            }

                            if (b != 1 && b != 2)
                                AddError(errors, key, "Value must be 1 (false) or 2 (true).");

                            break;
                        }

                    default:
                        AddError(errors, key, "Unsupported attribute type.");
                        break;
                }
            }

            if (errors.Count != 0)
            {
                throw new AttributeValidationException(
                    errors.ToDictionary(k => k.Key, v => v.Value.ToArray())
                );
            }

            var uploadedImages = await _image.Upload(request.Images, cancellationToken);

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
                PhoneNumber = request.PhoneNumber,
                UserId = 1,
                PromoType = request.PromoType,
                PromoDays = request.PromoDays,
                IsColored = request.IsColored,
                ColorDays = request.ColorDays,
                AutoRenewal = request.AutoRenewal,
                AutoRenewalOnceIn = request.AutoRenewalOnceIn,
                AutoRenewalAtTime = request.AutoRenewalAtTime
            };

            _context.Posts.Add(post);

            for (int i = 0; i < uploadedImages.Count; i++)
            {
                post.PostsImages.Add(new PostsImages
                {
                    Post = post,
                    Image = uploadedImages[i],
                    Order = i + 1
                });
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AddPostCommand failed: {ex}");
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
}