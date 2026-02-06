using AutoMapper;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Cities.Models;

public class CityDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public sealed class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CityEntity, CityDto>();
        }
    }
}
