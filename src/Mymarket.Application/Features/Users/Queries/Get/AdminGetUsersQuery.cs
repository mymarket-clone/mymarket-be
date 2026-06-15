using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Common;

namespace Mymarket.Application.Features.Users.Queries.Get;

public record AdminGetUsersQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    bool? IsBlocked = null
) : IRequest<PaginatedResult<AdminUserDto>>;

public class AdminGetUsersQueryHandler(
    IApplicationDbContext context) : IRequestHandler<AdminGetUsersQuery, PaginatedResult<AdminUserDto>>
{
    public async Task<PaginatedResult<AdminUserDto>> Handle(AdminGetUsersQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize is < 1 or > 100 ? 20 : request.PageSize;

        var query = context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim().ToLower();
            query = query.Where(x =>
                x.Firstname.ToLower().Contains(search) ||
                x.LastName.ToLower().Contains(search) ||
                x.Email.ToLower().Contains(search) ||
                x.PhoneNumber.Contains(search));
        }

        if (request.IsBlocked.HasValue)
        {
            query = query.Where(x => x.IsBlocked == request.IsBlocked.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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
                x.Balance,
                x.Posts.Count(),
                x.CreatedAt,
                x.UpdatedAt))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<AdminUserDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
