using Mymarket.Domain.Entities;
using Mapster;

namespace Mymarket.Application.Features.Cities.Models;

public class CityDto : IMapFrom<CityEntity>
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
