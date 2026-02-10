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
            .Property(x => x.Label)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.Label)
            .IsRequired();
    }
}
