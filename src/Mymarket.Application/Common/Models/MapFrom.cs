namespace Mymarket.Application.Common.Models;

using AutoMapper;
using Mymarket.Application.Interfaces;

public abstract class MapFrom<TEntity> : IMapFrom<TEntity>
{
    public virtual void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(TEntity), GetType());
    }
}