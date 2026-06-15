using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;
using Mymarket.Domain.Models;

namespace Mymarket.Application.Features.Users.Commands.RefreshUser;

public record RefreshUserCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<AuthDto>;

public class RefreshUserCommandHandler(
    IApplicationDbContext context,
    IAuthSessionService authSessionService) : IRequestHandler<RefreshUserCommand, AuthDto>
{
    public async Task<AuthDto> Handle(
        RefreshUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(
                x => x.RefreshToken == request.RefreshToken && x.RefreshTokenExpiry > DateTime.UtcNow,
                cancellationToken);

        if (user is null)
            throw new UnauthorizedAccessException(SharedResources.InvalidRefreshOrUser);

        if (user.IsBlocked)
            throw new UnauthorizedAccessException(SharedResources.InvalidRefreshOrUser);

        return await authSessionService.CreateSessionAsync(user, cancellationToken);
    }
}
