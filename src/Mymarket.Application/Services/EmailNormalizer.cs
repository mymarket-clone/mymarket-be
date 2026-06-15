using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Services;

public sealed class EmailNormalizer : IEmailNormalizer
{
    public string Normalize(string email) => email.Trim().ToUpperInvariant();
}
