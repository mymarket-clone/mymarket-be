using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Infrastructure.Configuration;

public class ListingServicePriceEntityConfiguration : IEntityTypeConfiguration<ListingServicePriceEntity>
{
    public void Configure(EntityTypeBuilder<ListingServicePriceEntity> builder)
    {
        builder.ToTable("ListingServicePrices");

        builder.Property(x => x.ServiceType).IsRequired();
        builder.Property(x => x.FromDay).IsRequired();
        builder.Property(x => x.ToDay).IsRequired();
        builder.Property(x => x.PricePerDay).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.OriginalPricePerDay).HasPrecision(18, 2);
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasIndex(x => new { x.ServiceType, x.FromDay, x.ToDay }).IsUnique();

        builder.HasData(
            new ListingServicePriceEntity { Id = 1, ServiceType = ListingServiceType.Vip, FromDay = 1, ToDay = 30, PricePerDay = 2.5m, OriginalPricePerDay = 2.5m, IsActive = true },
            new ListingServicePriceEntity { Id = 2, ServiceType = ListingServiceType.VipPlus, FromDay = 1, ToDay = 4, PricePerDay = 4m, OriginalPricePerDay = 4m, IsActive = true },
            new ListingServicePriceEntity { Id = 3, ServiceType = ListingServiceType.VipPlus, FromDay = 5, ToDay = 8, PricePerDay = 3.5m, OriginalPricePerDay = 4m, IsActive = true },
            new ListingServicePriceEntity { Id = 4, ServiceType = ListingServiceType.VipPlus, FromDay = 9, ToDay = 16, PricePerDay = 3.15m, OriginalPricePerDay = 4m, IsActive = true },
            new ListingServicePriceEntity { Id = 5, ServiceType = ListingServiceType.VipPlus, FromDay = 17, ToDay = 30, PricePerDay = 3m, OriginalPricePerDay = 4m, IsActive = true },
            new ListingServicePriceEntity { Id = 6, ServiceType = ListingServiceType.SuperVip, FromDay = 1, ToDay = 4, PricePerDay = 9m, OriginalPricePerDay = 9m, IsActive = true },
            new ListingServicePriceEntity { Id = 7, ServiceType = ListingServiceType.SuperVip, FromDay = 5, ToDay = 8, PricePerDay = 8m, OriginalPricePerDay = 9m, IsActive = true },
            new ListingServicePriceEntity { Id = 8, ServiceType = ListingServiceType.SuperVip, FromDay = 9, ToDay = 16, PricePerDay = 7.5m, OriginalPricePerDay = 9m, IsActive = true },
            new ListingServicePriceEntity { Id = 9, ServiceType = ListingServiceType.SuperVip, FromDay = 17, ToDay = 30, PricePerDay = 7m, OriginalPricePerDay = 9m, IsActive = true },
            new ListingServicePriceEntity { Id = 10, ServiceType = ListingServiceType.Color, FromDay = 1, ToDay = 8, PricePerDay = 0.3m, OriginalPricePerDay = 0.3m, IsActive = true },
            new ListingServicePriceEntity { Id = 11, ServiceType = ListingServiceType.Color, FromDay = 9, ToDay = 16, PricePerDay = 0.27m, OriginalPricePerDay = 0.3m, IsActive = true },
            new ListingServicePriceEntity { Id = 12, ServiceType = ListingServiceType.Color, FromDay = 17, ToDay = 30, PricePerDay = 0.25m, OriginalPricePerDay = 0.3m, IsActive = true },
            new ListingServicePriceEntity { Id = 13, ServiceType = ListingServiceType.AutoRenewal, FromDay = 1, ToDay = 30, PricePerDay = 0.25m, OriginalPricePerDay = 0.25m, IsActive = true }
        );
    }
}
