using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.features.Users.Queries.UserExists;

public record UserExistsQuery(
    string Email
): IRequest<bool>;

public class UserExistsQueryHandler(
    IApplicationDbContext context) : IRequestHandler<UserExistsQuery, bool>
{
    public async Task<bool> Handle(UserExistsQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Users
            .AsTracking()
            .AnyAsync(x => x.Email.ToLower().Equals(request.Email.ToLower()), cancellationToken);

        return result;
    }
}