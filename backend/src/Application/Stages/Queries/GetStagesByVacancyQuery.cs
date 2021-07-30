using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Application.Vacancies.Dtos;

namespace Application.Stages.Queries
{
    public class GetStagesByVacancyQuery : IRequest<ShortVacancyWithStagesDto>
    {
        public string VacancyId { get; }

        public GetStagesByVacancyQuery(string vacancyId)
        {
            VacancyId = vacancyId;
        }
    }

    public class GetStagesByVacancyQueryHandler
        : IRequestHandler<GetStagesByVacancyQuery, ShortVacancyWithStagesDto>
    {
        private readonly IStageReadRepository _repository;
        private readonly IMapper _mapper;

        public GetStagesByVacancyQueryHandler(IMapper mapper, IStageReadRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ShortVacancyWithStagesDto> Handle(GetStagesByVacancyQuery query, CancellationToken _)
        {
            Vacancy vacancy = await _repository.GetByVacancyAsync(query.VacancyId);

            return _mapper.Map<Vacancy, ShortVacancyWithStagesDto>(vacancy);
        }
    }
}
