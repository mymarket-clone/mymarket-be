using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class AttributeUnitEntityConfiguration : IEntityTypeConfiguration<AttributeUnitEntity>
{
    public void Configure(EntityTypeBuilder<AttributeUnitEntity> builder)
    {
        builder
            .Property(x => x.Name)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(x => x.NameEn)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(x => x.NameRu)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(255);

        builder
            .HasMany(x => x.Attributes)
            .WithOne(x => x.Unit)
            .HasForeignKey(x => x.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
