using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Queries;
using Application.ElasticEnities.Dtos;
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
        protected readonly IVacancyReadRepository _repository;
        protected readonly IStageReadRepository _stageRepo;
        protected readonly IReadRepository<Action> _readActionRepository;

        protected readonly IMapper _mapper;
        protected readonly ISender _mediator;

        public GetVacancyByIdQueryHandler(IVacancyReadRepository repository, IMapper mapper,
         IStageReadRepository stageRepo,
         ISender mediator,
         IReadRepository<Action> readActionRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _stageRepo = stageRepo;
            _mediator = mediator;
            _readActionRepository = readActionRepository;
        }

        public async Task<VacancyDto> Handle(GetVacancyByIdQuery query, CancellationToken _)
        {
            var tagsQueryTask = await _mediator.Send(new GetElasticDocumentByIdQuery<ElasticEnitityDto>(query.Id));
            var vacancy = await _repository.GetByCompanyIdAsync(query.Id);
            vacancy.Stages = (await _stageRepo.GetByVacancyId(query.Id)).ToList();
            var actions = (List<Action>)await _readActionRepository.GetEnumerableAsync();
            foreach (var stage in vacancy.Stages)
            {
                stage.Actions = actions.FindAll(x => x.StageId == stage.Id);
            }
            var vacancyDto = _mapper.Map<VacancyDto>(vacancy);
            vacancyDto.Tags = tagsQueryTask;
            return vacancyDto;
        }
    }
}