using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;
using System.Reflection;

namespace Mymarket.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IApplicationDbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<VerificationCode> VerificationCode => Set<VerificationCode>();
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken) => base.SaveChangesAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}