using MediatR;
using Microsoft.AspNetCore.Http;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Posts.Commands.Add;

public record AddPostCommand(
    PostType PostType,
    int CategoryId,
    string? YoutubeLink,
    ConditionType? ConditionType,
    List<IFormFile> Images,
    IFormFile MainImage,
    string Title,
    string Description,
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
    int? AutoRenewalAtTime
) : IRequest<Unit>;


public sealed class AddPostCommandHandler(
    IApplicationDbContext _context,
    IImageService _image) : IRequestHandler<AddPostCommand, Unit>
{
    public async Task<Unit> Handle(AddPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
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
}