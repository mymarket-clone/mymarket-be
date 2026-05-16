using Mapster;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Brands.Models;

public class BrandDto : IMapFrom<BrandEntity>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int LogoId { get; set; }
    public required string LogoUrl { get; set; }
}
