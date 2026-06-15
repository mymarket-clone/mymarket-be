using Mymarket.Domain.Common;
using Mymarket.Domain.Enums;

namespace Mymarket.Domain.Entities;

public class ListingServicePriceEntity : BaseEntity<int>
{
    public ListingServiceType ServiceType { get; set; }
    public int FromDay { get; set; }
    public int ToDay { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal? OriginalPricePerDay { get; set; }
    public bool IsActive { get; set; } = true;
}
