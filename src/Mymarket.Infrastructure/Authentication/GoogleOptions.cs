namespace Mymarket.Infrastructure.Authentication;

public sealed class GoogleOptions
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string PublicOrigin { get; set; } = string.Empty;
}
