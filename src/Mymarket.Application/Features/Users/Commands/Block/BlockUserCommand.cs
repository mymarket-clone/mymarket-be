using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Users.Commands.Block;

public record BlockUserCommand(
    int Id,
    bool Block
) : IRequest;

public class BlockUserCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<BlockUserCommand>
{
    public async Task Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Block && currentUser.Id == request.Id)
        {
            throw new ValidationException(
                [new ValidationFailure(nameof(request.Block), "Cannot block your own user.")]);
        }

        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        user.IsBlocked = request.Block;

        if (request.Block)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiry = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
