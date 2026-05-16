using Microsoft.AspNetCore.Authorization;
using Mymarket.Domain.Enums;

namespace Mymarket.Infrastructure.Authentication.Policies;

public class PermissionRequirement(
    Permissions permission,
    AccessLevelType accessLevel) : IAuthorizationRequirement
{
    public Permissions Permission { get; } = permission;
    public AccessLevelType AccessLevel { get; } = accessLevel;
}
