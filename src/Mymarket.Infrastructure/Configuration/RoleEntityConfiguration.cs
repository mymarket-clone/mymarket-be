using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.ToTable("Roles");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasMany(x => x.Permissions)
                    .WithMany(x => x.Roles)
                    .UsingEntity<Dictionary<string, object>>(
                        "RolePermissions",
                        j => j.HasOne<PermissionEntity>()
                              .WithMany()
                              .HasForeignKey("PermissionId"),
                        j => j.HasOne<RoleEntity>()
                              .WithMany()
                              .HasForeignKey("RoleId"),
                        j =>
                        {
                            j.HasKey("RoleId", "PermissionId");
                            j.ToTable("RolePermissions");
                        });
    }
}
