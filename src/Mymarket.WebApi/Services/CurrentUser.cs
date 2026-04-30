using Mymarket.Application.Interfaces;

namespace Mymarket.WebApi.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public int? Id
    {
        get
        {
            var strId = httpContextAccessor.HttpContext?.User?
                .FindFirst(Domain.Constants.ClaimTypes.Id)?
                .Value;

            return int.TryParse(strId, out var id) ? id : null;
        }
    }

    public Guid? SessionId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?
                .Request
                .Cookies[Domain.Constants.AnonClaimTypes.AnonSessionId];

            return Guid.TryParse(value, out var guid) ? guid : null;
        }
    }
}
