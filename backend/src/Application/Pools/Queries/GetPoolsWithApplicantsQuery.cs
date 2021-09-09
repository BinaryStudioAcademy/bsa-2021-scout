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
using Application.ElasticEnities.Dtos;
using Application.Interfaces;

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
        protected readonly ISender _mediator;
        private readonly ICurrentUserContext _context;

        public GetPoolWithApplicantsQueryHandler(IPoolReadRepository repository, ICurrentUserContext context, ISender mediator, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
        }

        public async Task<List<PoolDto>> Handle(GetPoolsWithApplicantsQuery query, CancellationToken _)
        {
            var companyId = (await _context.GetCurrentUser()).CompanyId;

            var result = await _repository.GetPoolsWithApplicantsAsync(companyId);

            return _mapper.Map<List<PoolDto>>(result);
        }
    }
}
