using Mymarket.Application.features.Users.Common.Models;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Interfaces;

public interface IAuthSessionService
{
    Task<AuthDto> CreateSessionAsync(UserEntity user, CancellationToken cancellationToken);
}
