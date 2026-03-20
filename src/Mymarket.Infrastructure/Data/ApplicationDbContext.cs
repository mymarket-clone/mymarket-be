using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;
using System.Reflection;

namespace Mymarket.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IApplicationDbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<VerificationCodeEntity> VerificationCode => Set<VerificationCodeEntity>();
    public DbSet<PostEntity> Posts => Set<PostEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<CategoryBrandsEntity> CategoryBrands => Set<CategoryBrandsEntity>();
    public DbSet<CityEntity> Cities => Set<CityEntity>();
    public DbSet<ImageEntity> Images => Set<ImageEntity>();
    public DbSet<AttributeEntity> Attributes => Set<AttributeEntity>();
    public DbSet<AttributeUnitEntity> AttributeUnits => Set<AttributeUnitEntity>();
    public DbSet<AttributeOptionsEntity> AttributesOptions => Set<AttributeOptionsEntity>();
    public DbSet<CategoryAttributesEntity> CategoryAttributes => Set<CategoryAttributesEntity>();
    public DbSet<PostAttributesEntity> PostAttributes => Set<PostAttributesEntity>();
    public DbSet<BrandEntity> Brands => Set<BrandEntity>();
    public DbSet<HomeCategoriesEntity> HomeCategories => Set<HomeCategoriesEntity>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken) => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }
}