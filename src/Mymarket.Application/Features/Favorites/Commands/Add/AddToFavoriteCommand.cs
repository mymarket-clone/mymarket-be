using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Favorites.Commands.Add;

public record AddToFavoriteCommand(
   int PostId
) : IRequest<Unit>;

public class AddToFavoriteCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<AddToFavoriteCommand, Unit>
{
    public async Task<Unit> Handle(AddToFavoriteCommand request, CancellationToken cancellationToken)
    {
        var existingFavorite = await context.Favorites
            .FirstOrDefaultAsync(x => x.PostId == request.PostId && x.UserId == (int)currentUser.Id!, cancellationToken);

        if (existingFavorite != null)
        {
            throw new ValidationException(SharedResources.RecordAlredyExists);
        }

            var favorite = new FavoritesEntity
        {
            UserId = (int)currentUser.Id!,
            PostId = request.PostId,
        };

        await context.Favorites.AddAsync(favorite, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

