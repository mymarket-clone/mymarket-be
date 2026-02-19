namespace Mymarket.Infrastructure.Authentication;

public class JwtOptions
{
    public string Secret { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int AccessTokenTtl { get; set; }
    public int RefreshTokenTtl { get; set; }
}
