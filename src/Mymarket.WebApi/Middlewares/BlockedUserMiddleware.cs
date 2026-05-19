using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Mymarket.Domain.Constants;
using Mymarket.Infrastructure.Data;

namespace Mymarket.WebApi.Middlewares;

public class BlockedUserMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
    {
        var userIdValue = context.User.FindFirst(ClaimTypes.Id)?.Value;

        if (!int.TryParse(userIdValue, out var userId))
        {
            await next(context);
            return;
        }

        var isBlocked = await dbContext.Users
            .AsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => x.IsBlocked)
            .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(5))
            .FirstOrDefaultAsync(context.RequestAborted);

        if (isBlocked)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(
                new
                {
                   message = "User is blocked"
                }, context.RequestAborted 
            );

            return;
        }

        await next(context);
    }
}
