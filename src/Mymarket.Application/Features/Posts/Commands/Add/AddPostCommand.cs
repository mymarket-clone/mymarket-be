using MediatR;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.features.Posts.Commands.CreatePost;

public record AddPostCommand(
    PostType PostType,
    int CategoryId,
    ConditionType? ConditionType,
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
    int UserId,
    PromoType? PromoType,
    bool IsColored,
    bool AutoRenewal
) : IRequest<Unit>;

public class AddPostCommandHandler(IApplicationDbContext _context) : IRequestHandler<AddPostCommand, Unit>
{
    public async Task<Unit> Handle(AddPostCommand request, CancellationToken cancellationToken)
    {
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
            UserId = request.UserId,
            PromoType = request.PromoType,
            IsColored = request.IsColored,
            AutoRenewal = request.AutoRenewal
        };

        _context.Posts.Add(post);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}