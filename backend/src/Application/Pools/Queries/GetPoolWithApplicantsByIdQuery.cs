using Application.Common.Exceptions;
using Application.ElasticEnities.Dtos;
using Application.Pools.Dtos;
using AutoMapper;
using Domain.Interfaces.Read;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        protected readonly ISender _mediator;

        public GetPoolWithApplicantsByIdQueryHandler(IPoolReadRepository repository, ISender mediator, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }


        public async Task<PoolDto> Handle(GetPoolWithApplicantsByIdQuery query, CancellationToken _)
        {
            var result = await _repository.GetPoolWithApplicantsByIdAsync(query.Id);

            if (result == null) throw new NotFoundException(typeof(PoolDto), query.Id);

            var ids = result.PoolApplicants.Select(x=> $"id={x.ApplicantId}");

            if (ids.Count() > 0)
            {
                var elasticQuery = string.Join(',', ids).Replace(",", " OR ");
                var tags = await _mediator.Send(new GetElasticDocumentsListBySearchRequestQuery<ElasticEnitityDto>(elasticQuery, _));
                var resDto = _mapper.Map<PoolDto>(result);

                resDto.Applicants.ToList().ForEach(applicant =>
                {
                    applicant.Tags = tags.FirstOrDefault(y => y.Id == applicant.Id);
                });
                return resDto;
            }



            return _mapper.Map<PoolDto>(result);
        }
    }
}
