using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Favorites.Commands.Add;

public class AddToFavoriteCommandValidator : AbstractValidator<FavoritesEntity>
{
    private readonly IApplicationDbContext _context;

    public AddToFavoriteCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.PostId)
            .NotEmpty().WithMessage(SharedResources.IdRequired)
            .MustAsync(PostExists).WithMessage(SharedResources.IdDoesnotExist);
    }

    public async Task<bool> PostExists(int postId, CancellationToken cancellationToken)
    {
        return await _context.Posts.AnyAsync(x => x.Id == postId, cancellationToken);
    }
}
