using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Queries;
using Application.ElasticEnities.Dtos;
using Application.Interfaces;
using Application.Users.Dtos;
using Application.Vacancies.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;

namespace Application.Vacancies.Queries
{
    public class GetVacancyByIdQuery : IRequest<VacancyDto>
    {
        public string Id { get; }

        public GetVacancyByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetVacancyByIdQueryHandler : IRequestHandler<GetVacancyByIdQuery, VacancyDto>
    {
        protected readonly IReadRepository<Vacancy> _vacancyRepo;
        protected readonly IStageReadRepository _stageRepo;

        protected readonly IMapper _mapper;
        protected readonly ISender _mediator;

        public GetVacancyByIdQueryHandler(IReadRepository<Vacancy> vacancyRepo, IStageReadRepository stageRepo,
         IMapper mapper,
         ISender mediator)
        {
            _vacancyRepo = vacancyRepo;
            _mapper = mapper;
            _stageRepo = stageRepo;
            _mediator = mediator;
        }

        public async Task<VacancyDto> Handle(GetVacancyByIdQuery query, CancellationToken _)
        {
            var tagsQueryTask = await _mediator.Send(new GetElasticDocumentByIdQuery<ElasticEnitityDto>(query.Id));
            Vacancy vacancy = await _vacancyRepo.GetAsync(query.Id);
            vacancy.Stages = (await _stageRepo.GetByVacancyAsync(query.Id)).Stages;
            var vacancyDto = _mapper.Map<VacancyDto>(vacancy);
            vacancyDto.Tags = tagsQueryTask;
            return vacancyDto;
        }
    }
}
