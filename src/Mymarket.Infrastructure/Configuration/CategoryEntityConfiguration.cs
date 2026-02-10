using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("Categories");

        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .HasColumnType("text")
            .IsRequired();

        builder
            .Property(x => x.NameEn)
            .HasMaxLength(100)
            .HasColumnType("text");

        builder
            .Property(x => x.NameRu)
            .HasMaxLength(100)
            .HasColumnType("text");

        builder
            .Property(x => x.CategoryPostType)
            .IsRequired();

        builder
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
