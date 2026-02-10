using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class AttributesEntityConfiguration : IEntityTypeConfiguration<AttributesEntity>
{
    public void Configure(EntityTypeBuilder<AttributesEntity> builder)
    {
        builder.ToTable("Attributes");

        builder
            .Property(x => x.Name)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.NameEn)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.NameRu)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.Code)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.AttributeType)
            .IsRequired();
    }
}
