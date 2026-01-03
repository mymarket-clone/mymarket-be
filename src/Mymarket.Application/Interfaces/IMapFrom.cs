using AutoMapper;

namespace Mymarket.Application.Interfaces;

public interface IMapFrom<TEntity>
{
    void Mapping(Profile profile);
}
