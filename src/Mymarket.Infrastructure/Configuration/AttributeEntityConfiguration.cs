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
            .HasMaxLength(255);

        builder
            .Property(x => x.NameRu)
            .HasColumnType("text")
            .HasMaxLength(255);

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

        builder
            .HasOne(x => x.Unit)
            .WithMany(x => x.Attributes)
            .HasForeignKey(x => x.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.PostAttributes)
            .WithOne(x => x.Attribute)
            .HasForeignKey(x => x.AttributeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
