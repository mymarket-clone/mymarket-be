using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Infrastructure.Configuration;

public class PermissionEntityConfiguration : IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasData(
            Enum.GetValues<Permissions>()
                .Select(permission =>
                    new PermissionEntity
                    {
                        Id = (int)permission,
                        Name = permission.ToString()
                    })
        );
    }
}
