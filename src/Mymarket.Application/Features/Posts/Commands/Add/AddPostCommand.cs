using MediatR;
using Microsoft.AspNetCore.Http;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Services;

namespace Mymarket.Application.Features.Posts.Commands.Add;

public sealed record AddPostCommand(
    PostType PostType,
    int CategoryId,
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
    string Name,
    string PhoneNumber,
    PromoType? PromoType,
    int? PromoDays,
    bool IsColored,
    int? ColorDays,
    bool AutoRenewal,
    int? AutoRenewalOnceIn,
    int? AutoRenewalAtTime
) : IRequest<Unit>;

public sealed class AddPostCommandHandler(IApplicationDbContext _context, ImageService _image) : IRequestHandler<AddPostCommand, Unit>
{
    public async Task<Unit> Handle(AddPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _image.Upload(request.Images, cancellationToken);

            var post = new PostEntity
            {
                PostType = request.PostType,
                CategoryId = request.CategoryId,
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
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                UserId = 1,
                PromoType = request.PromoType,
                IsColored = request.IsColored,
                AutoRenewal = request.AutoRenewal
            };

            _context.Posts.Add(post);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        catch
        {
            throw new Exception("An error occurred while adding the post.");
        }
    }
}