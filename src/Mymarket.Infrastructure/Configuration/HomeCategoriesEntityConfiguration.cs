using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class HomeCategoriesEntityConfiguration : IEntityTypeConfiguration<HomeCategoriesEntity>
{
    public void Configure(EntityTypeBuilder<HomeCategoriesEntity> builder)
    {
        builder.ToTable("HomeCategories");

        builder
            .Property(x => x.CategoryId)
            .IsRequired();
    }
}
