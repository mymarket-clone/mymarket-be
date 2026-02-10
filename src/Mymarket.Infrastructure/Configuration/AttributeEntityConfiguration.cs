using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class AttributeEntityConfiguration : IEntityTypeConfiguration<AttributeEntity>
{
    public void Configure(EntityTypeBuilder<AttributeEntity> builder)
    {
        builder.ToTable("Attributes");

        builder
            .Property(x => x.Name)
            .HasColumnType("text")
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.NameEn)
            .HasColumnType("text")
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.NameRu)
            .HasColumnType("text")
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.Code)
            .HasColumnType("text")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder
            .Property(x => x.AttributeType)
            .IsRequired();
    }
}
