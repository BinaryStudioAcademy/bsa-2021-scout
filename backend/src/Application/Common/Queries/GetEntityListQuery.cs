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
    public class GetEntityListQuery<TDto> : IRequest<IEnumerable<TDto>>
        where TDto : Dto
    { }

    public class GetEntityListQueryHandler<TEntity, TDto> : IRequestHandler<GetEntityListQuery<TDto>, IEnumerable<TDto>>
        where TEntity : Entity
        where TDto : Dto
    {
        protected readonly IReadRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GetEntityListQueryHandler(IReadRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> Handle(GetEntityListQuery<TDto> query, CancellationToken _)
        {
            IEnumerable<TEntity> result = await _repository.GetEnumerableAsync();
            //TODO Get by company Id
            return _mapper.Map<IEnumerable<TDto>>(result);
        }
    }
}
