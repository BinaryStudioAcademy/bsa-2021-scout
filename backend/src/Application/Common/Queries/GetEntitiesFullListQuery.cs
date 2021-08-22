using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Application.Common.Models;

namespace Application.Common.Queries
{
    public class GetEntitiesFullListQuery<TEntity> : IRequest<IEnumerable<TEntity>>
        where TEntity : Entity
    { }

    public class GetEntitiesFullListQueryHandler<TEntity>
        where TEntity : Entity        
    {
        protected readonly IReadRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GetEntitiesFullListQueryHandler(IReadRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TEntity>> Handle(GetEntitiesFullListQuery<TEntity> query, CancellationToken _)
        {
            IEnumerable<TEntity> result = await _repository.GetEnumerableAsync();

            return result;
        }
    }
}
