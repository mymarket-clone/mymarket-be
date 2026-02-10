using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class CategoryAttributesEntityConfiguration : IEntityTypeConfiguration<CategoryAttributesEntity>
{
    public void Configure(EntityTypeBuilder<CategoryAttributesEntity> builder)
    {
        builder.ToTable("CategoryAttributes");

        builder
            .Property(x => x.CategoryId)
            .IsRequired();

        builder
            .Property(x => x.AttributeId)
            .IsRequired();

        builder
            .Property(x => x.IsRequired)
            .IsRequired();

        builder
            .Property(x => x.Order)
            .IsRequired();
    }
}
