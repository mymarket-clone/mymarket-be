using Mymarket.Domain.Models;

namespace Mymarket.Application.Interfaces;

public interface ITokenProvider
{
    (string, DateTime) CreateAccessToken(UserModel user);
    string CreateRefreshToken();
}
