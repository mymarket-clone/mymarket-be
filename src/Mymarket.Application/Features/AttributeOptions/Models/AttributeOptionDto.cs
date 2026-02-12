using AutoMapper;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.AttributeOptions.Models;

public class AttributeOptionDto
{
    public int Id { get; set; }
    public int Order { get; set; }
    public required string Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameRu { get; set; }
    public sealed class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AttributesOptionsEntity, AttributeOptionDto>();
        }
    }
}
