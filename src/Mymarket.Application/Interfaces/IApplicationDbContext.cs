using Microsoft.EntityFrameworkCore;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<VerificationCodeEntity> VerificationCode { get; }
    DbSet<PostEntity> Posts { get; }
    DbSet<CategoryEntity> Categories { get; }
    DbSet<CityEntity> Cities { get; }
    DbSet<ImageEntity> Images { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
