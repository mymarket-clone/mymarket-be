using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Users.Queries.GetAdminById;

public record AdminGetUserByIdQuery(int Id) : IRequest<AdminUserDto>;

public class AdminGetUserByIdQueryHandler(
    IApplicationDbContext context) : IRequestHandler<AdminGetUserByIdQuery, AdminUserDto>
{
    public async Task<AdminUserDto> Handle(AdminGetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new AdminUserDto(
                x.Id,
                x.Firstname,
                x.LastName,
                x.Email,
                x.Gender,
                x.BirthYear,
                x.PhoneNumber,
                x.EmailVerified,
                x.IsBlocked,
                x.AccessLevel,
                x.Posts.Count(),
                x.CreatedAt,
                x.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KeyNotFoundException(SharedResources.RecordNotFound);

        return user;
    }
}
