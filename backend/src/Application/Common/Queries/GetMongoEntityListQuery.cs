using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces;
using Application.Common.Models;

namespace Application.Common.Queries
{
    public class GetMongoEntityListQuery<TDto> : IRequest<IEnumerable<TDto>>
        where TDto : MongoDto
    { }

    public class GetMongoEntityListQuery<TEntity, TDto> : IRequestHandler<GetMongoEntityListQuery<TDto>, IEnumerable<TDto>>
        where TEntity : MongoEntity
        where TDto : MongoDto
    {
        protected readonly IMongoReadRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GetMongoEntityListQuery(IMongoReadRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> Handle(GetMongoEntityListQuery<TDto> query, CancellationToken _)
        {
            IEnumerable<TEntity> result = await _repository.GetEnumerableAsync();

            return _mapper.Map<IEnumerable<TDto>>(result);
        }
    }
}
