using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities.Category;

namespace Mymarket.Infrastructure.Configuration.Category;

internal class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryEntityTranslations>
{
    public void Configure(EntityTypeBuilder<CategoryEntityTranslations> builder)
    {
        builder.ToTable("CategoriesTranslations");

        builder.Property(x => x.LanguageCode)
               .IsRequired()
               .HasMaxLength(2);

        builder.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(128);

        builder.HasIndex(x => new { x.CategoryId, x.LanguageCode })
               .IsUnique();
    }
}
