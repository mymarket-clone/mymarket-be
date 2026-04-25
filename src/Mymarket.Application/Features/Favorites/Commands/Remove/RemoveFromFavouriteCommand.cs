using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Favorites.Commands.Remove;

public record RemoveFromFavouriteCommand(
    int PostId
) : IRequest<Unit>;

public class RemoveFromFavouriteCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<RemoveFromFavouriteCommand, Unit>
{
    public async Task<Unit> Handle(RemoveFromFavouriteCommand request, CancellationToken cancellationToken)
    {
        var record = await context.Favorites
            .FirstOrDefaultAsync(x => x.PostId == request.PostId && x.UserId == (int)currentUser.Id!, cancellationToken);

        if (record is not null)
        {
            context.Favorites.Remove(record);
            await context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}
