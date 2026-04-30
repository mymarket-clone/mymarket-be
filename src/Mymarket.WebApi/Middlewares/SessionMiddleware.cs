namespace Mymarket.WebApi.Middlewares;

public class SessionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var key = Domain.Constants.AnonClaimTypes.AnonSessionId;

        if (!context.Request.Cookies.ContainsKey(key))
        {
            var sid = Guid.NewGuid().ToString();

            context.Response.Cookies.Append(key, sid, new CookieOptions
            {
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });
        }

        await next(context);
    }
}
