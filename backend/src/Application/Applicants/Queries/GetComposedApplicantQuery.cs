using MediatR;
using AutoMapper;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Queries;
using Application.Applicants.Dtos;
using Domain.Interfaces.Read;
using Application.ElasticEnities.Dtos;
using System.Collections.Generic;
using Domain.Interfaces.Abstractions;

namespace Application.Applicants.Queries
{
    public class GetComposedApplicantQuery : IRequest<ApplicantDto>
    {
        public string Id { get; private set; }

        public GetComposedApplicantQuery(string id)
        {
            Id = id;
        }
    }

    public class GetComposedApplicantQueryHandler : IRequestHandler<GetComposedApplicantQuery, ApplicantDto>
    {
        private readonly ISender _mediator;
        private readonly IApplicantReadRepository _repository;
        private readonly IMapper _mapper;

        public GetComposedApplicantQueryHandler(IApplicantReadRepository repository, ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ApplicantDto> Handle(GetComposedApplicantQuery query, CancellationToken _)
        {
            var tagsQueryTask = _mediator.Send(new GetElasticDocumentByIdQuery<ElasticEnitityDto>(query.Id));

            var applicant = await _repository.GetByIdAsync(query.Id);

            var applicantDto = _mapper.Map<ApplicantDto>(applicant);

            applicantDto.Tags = await tagsQueryTask;
            applicantDto.Vacancies = _mapper.Map<IEnumerable<ApplicantVacancyInfoDto>>(await _repository.GetApplicantVacancyInfoListAsync(query.Id));

            return applicantDto;
        }
    }
}