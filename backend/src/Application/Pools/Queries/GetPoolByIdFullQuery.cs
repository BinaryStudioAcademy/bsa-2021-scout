using Application.Pools.Dtos;
using Application.Common.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;

namespace Application.Pools.Queries.GetPoolByIdQueryFull
{
    public class GetPoolByIdQueryFullHandler : GetEntitiesQueryHandler<Pool, PoolDto>
    {
        public GetPoolByIdQueryFullHandler(IReadRepository<Pool> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
