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

namespace Application.Common.Queries
{
    public class GetPoolWithApplicantsByIdQuery : IRequest<PoolDto>
    {
        public string Id { get; }
        public GetPoolWithApplicantsByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetPoolWithApplicantsByIdQueryHandler : IRequestHandler<GetPoolWithApplicantsByIdQuery, PoolDto>
    {
        protected readonly IPoolReadRepository _repository;
        protected readonly IMapper _mapper;

        public GetPoolWithApplicantsByIdQueryHandler(IPoolReadRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PoolDto> Handle(GetPoolWithApplicantsByIdQuery query, CancellationToken _)
        {
            var result = await _repository.GetPoolWithApplicantsByIdAsync(query.Id);

            if(result == null) throw new NotFoundException(typeof(PoolDto), query.Id);
            
            return _mapper.Map<PoolDto>(result);
        }
    }
}
