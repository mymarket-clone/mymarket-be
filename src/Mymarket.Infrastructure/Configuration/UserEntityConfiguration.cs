using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasIndex(x => x.PhoneNumber).IsUnique();

        builder.Property(x => x.Firstname)
            .IsRequired()
            .HasColumnType("text")
            .HasMaxLength(255);

        builder.Property(x => x.LastName)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Email)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Gender)
            .IsRequired();

        builder.Property(x => x.BirthYear)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(x => x.PasswordHash)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.EmailVerified)
            .IsRequired();

        builder
            .HasMany(x => x.Posts)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.PostViews)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserRoles",
                j => j.HasOne<RoleEntity>()
                      .WithMany()
                      .HasForeignKey("RoleId"),
                j => j.HasOne<UserEntity>()
                      .WithMany()
                      .HasForeignKey("UserId"),
                j =>
                {
                    j.HasKey("UserId", "RoleId");
                    j.ToTable("UserRoles");
                });
    }
}

