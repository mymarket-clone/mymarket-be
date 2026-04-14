using MediatR;
using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Microsoft.EntityFrameworkCore;

namespace Mymarket.Application.Features.Users.Queries.GetCurrent;

public record GetCurrentUserQuery : IRequest<UserDto>;

public class GetCurrentUserQueryHanlder(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == currentUser.Id, cancellationToken)
            ?? throw new KeyNotFoundException(SharedResources.RecordNotFound);

        var userDto = new UserDto(
            Id: user.Id,
            Name: user.Firstname,
            Lastname: user.LastName,
            Email: user.Email,
            EmailVerified: user.EmailVerified
        );

        return userDto;
    }
}
