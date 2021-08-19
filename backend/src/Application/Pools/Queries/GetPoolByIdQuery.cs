using Application.Pools.Dtos;
using Application.Common.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;

namespace Application.Pools.Queries.GetPoolById
{
    public class GetPoolByIdQueryHandler : GetEntitiesQueryHandler<Pool, PoolDto>
    {
        public GetPoolByIdQueryHandler(IReadRepository<Pool> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
