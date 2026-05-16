using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Mymarket.Domain.Enums;

namespace Mymarket.Infrastructure.Authentication.Policies;

public sealed class PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("permission:"))
        {
            var policyParts = policyName.Split(':');
            var permission = int.Parse(policyParts[1]);
            var accessLevel = policyParts.Length > 2
                ? int.Parse(policyParts[2])
                : (int)AccessLevelType.Admin;

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement((Permissions)permission, (AccessLevelType)accessLevel))
                .Build();

            return policy;
        }

        return await base.GetPolicyAsync(policyName);
    }
}
