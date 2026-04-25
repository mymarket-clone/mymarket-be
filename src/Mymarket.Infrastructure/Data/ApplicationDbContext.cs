using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Common;
using Mymarket.Domain.Entities;
using System.Reflection;

namespace Mymarket.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IApplicationDbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<VerificationCodeEntity> VerificationCode => Set<VerificationCodeEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<CategoryAttributesEntity> CategoryAttributes => Set<CategoryAttributesEntity>();
    public DbSet<CategoryBrandsEntity> CategoryBrands => Set<CategoryBrandsEntity>();
    public DbSet<BrandEntity> Brands => Set<BrandEntity>();
    public DbSet<CityEntity> Cities => Set<CityEntity>();
    public DbSet<PostEntity> Posts => Set<PostEntity>();
    public DbSet<PostAttributesEntity> PostAttributes => Set<PostAttributesEntity>();
    public DbSet<PostsImagesEntity> PostsImages => Set<PostsImagesEntity>();
    public DbSet<PostViewEntity> PostViews => Set<PostViewEntity>();
    public DbSet<ImageEntity> Images => Set<ImageEntity>();
    public DbSet<AttributeEntity> Attributes => Set<AttributeEntity>();
    public DbSet<AttributeUnitEntity> AttributeUnits => Set<AttributeUnitEntity>();
    public DbSet<AttributeOptionsEntity> AttributeOptions => Set<AttributeOptionsEntity>();
    public DbSet<HomeCategoriesEntity> HomeCategories => Set<HomeCategoriesEntity>();
    public DbSet<FavoritesEntity> Favorites => Set<FavoritesEntity>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }
}