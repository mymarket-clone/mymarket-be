using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Users.Queries.UserExists;
public record UserExistsQuery(string Email): IRequest<bool>;

public class UserExistsQueryHandler(IApplicationDbContext _context) : IRequestHandler<UserExistsQuery, bool>
{
    public async Task<bool> Handle(UserExistsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsTracking()
            .AnyAsync(x => x.Email.ToLower().Equals(request.Email.ToLower()), cancellationToken);
    }
}