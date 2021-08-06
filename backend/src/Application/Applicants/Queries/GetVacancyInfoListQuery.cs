using MediatR;
using AutoMapper;
using System.Linq;
using Domain.Entities;
using System.Threading;
using Domain.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Applicants.Dtos;

namespace Application.Applicants.Queries
{
    public class GetVacancyInfoListQuery : IRequest<IEnumerable<ApplicantVacancyInfoDto>>
    {
        public string Id { get; private set; }

        public GetVacancyInfoListQuery(string id)
        {
            Id = id;
        }
    }

    public class GetVacancyInfoListQueryHandler : IRequestHandler<GetVacancyInfoListQuery, IEnumerable<ApplicantVacancyInfoDto>>
    {
        private readonly IReadRepository<Vacancy> _vacancyRepository;
        private readonly IReadRepository<VacancyCandidate> _candidateRepository;
        private readonly IReadRepository<Stage> _stageRepository;

        public GetVacancyInfoListQueryHandler(
            IReadRepository<Vacancy> vacancyRepository,
            IReadRepository<VacancyCandidate> candidateRepository,
            IReadRepository<Stage> stageRepository)
        { }

        public async Task<IEnumerable<ApplicantVacancyInfoDto>> Handle(GetVacancyInfoListQuery query, CancellationToken _)
        {
            var vacancyList = await _vacancyRepository.GetEnumerableAsync();
            var candidateList = await _candidateRepository.GetEnumerableAsync();
            var stageList = await _stageRepository.GetEnumerableAsync();

            var filteredCandidateList = candidateList.Where(c => c.Id == query.Id);

            return filteredCandidateList.Join(
                stageList.Join(
                    vacancyList,
                    s => s.VacancyId,
                    v => v.Id,
                    (s, v) => new
                    {
                        VacancyTitle = v.Title,
                        StageName = s.Name,
                        StageId = s.Id
                    }
                ),
                c => c.StageId,
                u => u.StageId,
                (c, u) => new ApplicantVacancyInfoDto()
                {
                    Title = u.VacancyTitle,
                    Stage = u.StageName
                }
            );
        }
    }
}