using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Domain.Entities;
using Domain.Interfaces.Read;
using Application.Common.Exceptions;
using Application.Pools.Dtos;
using System.Collections.Generic;

namespace Application.Common.Queries
{
    public class GetPoolsWithApplicantsQuery : IRequest<List<PoolDto>>
    {
        public GetPoolsWithApplicantsQuery()
        {            
        }
    }

    public class GetPoolWithApplicantsQueryHandler : IRequestHandler<GetPoolsWithApplicantsQuery, List<PoolDto>>
    {
        protected readonly IPoolReadRepository _repository;
        protected readonly IMapper _mapper;

        public GetPoolWithApplicantsQueryHandler(IPoolReadRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PoolDto>> Handle(GetPoolsWithApplicantsQuery query, CancellationToken _)
        {
            var result = await _repository.GetPoolsWithApplicantsAsync();

            return _mapper.Map<List<PoolDto>>(result);
        }
    }
}
