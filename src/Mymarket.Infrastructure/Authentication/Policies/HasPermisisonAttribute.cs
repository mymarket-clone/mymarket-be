using Microsoft.AspNetCore.Authorization;
using Mymarket.Domain.Enums;

namespace Mymarket.Infrastructure.Authentication.Policies;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class HasPermissionAttribute(
    Permissions permissions,
    AccessLevelType accessLevel = AccessLevelType.Admin)
    : AuthorizeAttribute(policy: $"permission:{(int)permissions}:{(int)accessLevel}")
{

}
