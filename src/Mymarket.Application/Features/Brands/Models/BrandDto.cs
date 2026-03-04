using AutoMapper;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Brands.Models;

public class BrandDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int LogoId { get; set; }
    public required string LogoUrl { get; set; }
    public sealed class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<BrandEntity, BrandDto>();
        }
    }
}
