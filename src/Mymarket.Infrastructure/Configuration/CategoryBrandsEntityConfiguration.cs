using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class CategoryBrandsEntityConfiguration : IEntityTypeConfiguration<CategoryBrandsEntity>
{
    public void Configure(EntityTypeBuilder<CategoryBrandsEntity> builder)
    {
        builder.ToTable("CategoryBrands");

        builder.Property(x => x.CategoryId)
            .IsRequired();

        builder.Property(x => x.BrandId)
            .IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Brand)
            .WithMany()
            .HasForeignKey(x => x.BrandId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.CategoryId, x.BrandId })
            .IsUnique();
    }
}
