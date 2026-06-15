using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Users.Commands.TopUpBalance;

public record TopUpBalanceCommand(decimal Amount) : IRequest<UserDto>;

public class TopUpBalanceCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<TopUpBalanceCommand, UserDto>
{
    public async Task<UserDto> Handle(TopUpBalanceCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == currentUser.Id, cancellationToken)
            ?? throw new KeyNotFoundException(SharedResources.RecordNotFound);

        user.Balance += request.Amount;

        await context.SaveChangesAsync(cancellationToken);

        var favoritesCount = await context.Favorites.CountAsync(x => x.UserId == user.Id, cancellationToken);

        return new UserDto(
            Id: user.Id,
            Name: user.Firstname,
            Lastname: user.LastName,
            Email: user.Email,
            EmailVerified: user.EmailVerified,
            FavoritesCount: favoritesCount,
            Number: user.PhoneNumber,
            GenderType: user.Gender == GenderType.Male ? GenderType.Male : GenderType.Female,
            BirthYear: user.BirthYear,
            IsBlocked: user.IsBlocked,
            Balance: user.Balance
        );
    }
}
