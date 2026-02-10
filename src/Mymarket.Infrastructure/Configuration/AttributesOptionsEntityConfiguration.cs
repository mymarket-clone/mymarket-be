using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class AttributesOptionsEntityConfiguration : IEntityTypeConfiguration<AttributesOptionsEntity>
{
    public void Configure(EntityTypeBuilder<AttributesOptionsEntity> builder)
    {
        builder.ToTable("AttributesOptions");

        builder
            .Property(x => x.AttributeId)
            .IsRequired();

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
    }
}
