using Microsoft.AspNetCore.Authorization;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Enums;
using System.Globalization;

namespace Mymarket.Infrastructure.Authentication.Policies;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var accessLevelClaim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.AccessLevel);

        if (accessLevelClaim == null ||
            !int.TryParse(accessLevelClaim.Value, CultureInfo.InvariantCulture, out var accessLevelValue))
        {
            context.Fail(new AuthorizationFailureReason(this, "Must be admin to access this resource"));
            return Task.CompletedTask;
        }

        var accessLevel = (AccessLevelType)accessLevelValue;

        if (accessLevel < requirement.AccessLevel)
        {
            context.Fail(new AuthorizationFailureReason(this, $"Must be {requirement.AccessLevel} to access this resource"));
            return Task.CompletedTask;
        }

        if (requirement.Permission == default)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var permissions = context.User.Claims
                .Where(x => x.Type == ClaimTypes.Permission)
                .Select(x => x.Value)
                .ToHashSet();

        var requiredPermission = ((int)requirement.Permission).ToString(CultureInfo.InvariantCulture);
        var hasPermission = permissions.Contains(requiredPermission);

        if (hasPermission)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        context.Fail(new AuthorizationFailureReason(this, $"Missing permission {requirement.Permission}"));
        return Task.CompletedTask;
    }
}
