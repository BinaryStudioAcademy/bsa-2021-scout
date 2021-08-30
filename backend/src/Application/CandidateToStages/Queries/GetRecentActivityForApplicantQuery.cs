using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Application.CandidateToStages.Dtos;

namespace Application.CandidateToStages.Queries
{
    public class GetRecentActivityForApplicantQuery : IRequest<IEnumerable<VacancyWithRecentActivityDto>>
    {
        public string ApplicantId { get; set; }

        public GetRecentActivityForApplicantQuery(string applicantId)
        {
            ApplicantId = applicantId;
        }
    }

    public class GetRecentActivityForApplicantQueryHandler
        : IRequestHandler<GetRecentActivityForApplicantQuery, IEnumerable<VacancyWithRecentActivityDto>>
    {
        private readonly ICandidateToStageReadRepository _repository;
        private readonly IMapper _mapper;

        public GetRecentActivityForApplicantQueryHandler(ICandidateToStageReadRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VacancyWithRecentActivityDto>> Handle(
            GetRecentActivityForApplicantQuery query,
            CancellationToken _
        )
        {
            var candidateToStages =
                await _repository.GetRecentForApplicantAsync(query.ApplicantId);

            IEnumerable<IGrouping<string, CandidateToStage>> groups = candidateToStages
                .GroupBy(cts => cts.Stage.Vacancy.Id);

            IEnumerable<VacancyWithRecentActivityDto> info = new List<VacancyWithRecentActivityDto>();

            foreach (IGrouping<string, CandidateToStage> group in groups)
            {
                VacancyWithRecentActivityDto vacancy = new VacancyWithRecentActivityDto
                {
                    Activity = new List<CandidateToStageApplicantRecentActivityDto>(),
                };

                bool first = true;

                foreach (CandidateToStage cts in group)
                {
                    if (first)
                    {
                        vacancy.Id = cts.Stage.Vacancy.Id;
                        vacancy.Title = cts.Stage.Vacancy.Title;
                        vacancy.ProjectName = cts.Stage.Vacancy.Project.Name;
                        first = false;
                    }

                    vacancy.Activity = vacancy.Activity.Append(
                        _mapper.Map<CandidateToStage, CandidateToStageApplicantRecentActivityDto>(cts)
                    );
                }

                vacancy.Activity = vacancy.Activity.OrderByDescending(act => act.DateAdded);
                info = info.Append(vacancy);
            }

            return info;
        }
    }
}
