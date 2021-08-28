using Application.Common.Queries;
using Application.ElasticEnities.Dtos;
using Application.Vacancies.Dtos;
using AutoMapper;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Action = Domain.Entities.Action;

namespace Application.Vacancies.Queries
{
    public class GetVacancyByIdNoAuth : IRequest<VacancyDto>
    {
        public string Id { get; }

        public GetVacancyByIdNoAuth(string id)
        {
            Id = id;
        }
    }

    public class GetVacancyByIdNoAuthHandler : IRequestHandler<GetVacancyByIdNoAuth, VacancyDto>
    {
        protected readonly IVacancyReadRepository _repository;
        protected readonly IStageReadRepository _stageRepo;
        protected readonly IReadRepository<Action> _readActionRepository;

        protected readonly IMapper _mapper;
        protected readonly ISender _mediator;

        public GetVacancyByIdNoAuthHandler(IVacancyReadRepository repository, IMapper mapper,
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

        public async Task<VacancyDto> Handle(GetVacancyByIdNoAuth query, CancellationToken _)
        {
            var tagsQueryTask = await _mediator.Send(new GetElasticDocumentByIdQuery<ElasticEnitityDto>(query.Id));
            var vacancy = await _repository.GetAsync(query.Id);
            var vacancyDto = _mapper.Map<VacancyDto>(vacancy);
            vacancyDto.Tags = tagsQueryTask;
            return vacancyDto;
        }
    }

}
