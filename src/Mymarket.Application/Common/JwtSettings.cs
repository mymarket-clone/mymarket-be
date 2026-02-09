namespace Mymarket.Application.Common;

public class JwtSettings
{
    public required string SecretKey { get; set; }
    public required int AccessTokenTtl { get; set; }
    public required int RefreshTokenTtl { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
}
