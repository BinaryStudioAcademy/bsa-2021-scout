using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Application.Stages.Dtos;

namespace Application.Stages.Queries
{
    public class GetStagesByVacancyQuery : IRequest<IEnumerable<StageWithCandidatesDto>>
    {
        public string VacancyId { get; }

        public GetStagesByVacancyQuery(string vacancyId)
        {
            VacancyId = vacancyId;
        }
    }

    public class GetStagesByVacancyQueryHandler
        : IRequestHandler<GetStagesByVacancyQuery, IEnumerable<StageWithCandidatesDto>>
    {
        private readonly IStageReadRepository _repository;
        private readonly IMapper _mapper;

        public GetStagesByVacancyQueryHandler(IMapper mapper, IStageReadRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StageWithCandidatesDto>> Handle(GetStagesByVacancyQuery query, CancellationToken _)
        {
            IEnumerable<Stage> stages = await _repository.GetByVacancy(query.VacancyId);

            return stages.Select(_mapper.Map<Stage, StageWithCandidatesDto>);
        }
    }
}
