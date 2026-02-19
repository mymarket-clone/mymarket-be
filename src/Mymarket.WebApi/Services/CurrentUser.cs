using Microsoft.AspNet.Identity;
using Mymarket.Application.Interfaces;
using System.Security.Claims;

namespace Mymarket.WebApi.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public int? Id
    {
        get
        {
            var strId = httpContextAccessor.HttpContext?.User?.FindFirstValue(Domain.Constants.ClaimTypes.Id);
            return int.TryParse(strId, out var id) ? id : null;
        }
    }
}
