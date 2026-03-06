using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class BrandEntityConfiguration : IEntityTypeConfiguration<BrandEntity>
{
    public void Configure(EntityTypeBuilder<BrandEntity> builder)
    {
        builder.ToTable("Brands");

        builder
            .Property(x => x.Name)
            .HasColumnType("text")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasOne(x => x.Logo)
            .WithOne()
            .HasForeignKey<BrandEntity>(x => x.LogoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
