using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class ImageEntityConfiguration : IEntityTypeConfiguration<ImageEntity>
{
    public void Configure(EntityTypeBuilder<ImageEntity> builder)
    {
        builder.ToTable("Images");

        builder
            .Property(x => x.Url)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(500);

        builder
            .Property(x => x.UniqueId)
            .IsRequired();
    }
}