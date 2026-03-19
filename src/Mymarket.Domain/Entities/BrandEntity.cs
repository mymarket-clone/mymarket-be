using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class BrandEntity : BaseEntity<int>
{
    public required string Name { get; set; }
    public int LogoId { get; set; }
    public ImageEntity Logo { get; set; } = default!;
}