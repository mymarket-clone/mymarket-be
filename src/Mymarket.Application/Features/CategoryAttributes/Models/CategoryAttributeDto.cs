using AutoMapper;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.CategoryAttributes.Models;

public class CategoryAttributeDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int AttributeId { get; set; }
    public bool IsRequired { get; set; } = false;
    public int Order { get; set; }

    public sealed class Mapper : Profile
    {
        public Mapper() 
        { 
            CreateMap<CategoryAttributesEntity, CategoryAttributeDto>();
        }
    }
}
