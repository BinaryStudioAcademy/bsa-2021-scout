using Application.Home.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces.Read;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Home.Queries
{
    public class GetHotVacancySummaryQuery : IRequest<IEnumerable<HotVacancySummaryDto>>
    {

    }
    public class GetHotVacancySummaryQueryHandler : IRequestHandler<GetHotVacancySummaryQuery, IEnumerable<HotVacancySummaryDto>>
    {
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IHomeDataReadRepository _homePageInfoReadRepository;
        protected readonly IMapper _mapper;

        public GetHotVacancySummaryQueryHandler(ICurrentUserContext currentUserContext, IHomeDataReadRepository homePageInfoReadRepository,
                                            IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _homePageInfoReadRepository = homePageInfoReadRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HotVacancySummaryDto>> Handle(GetHotVacancySummaryQuery query, CancellationToken _)
        {
            var hrLead = await _currentUserContext.GetCurrentUser();

            var hotVacansies = await _homePageInfoReadRepository.GetHotVacancySummaryAsync(hrLead.CompanyId);

            var hotVacansiesDto = hotVacansies.GroupBy(p => p.Id)
                .Select(g =>
                {
                    var hotVacancy = new HotVacancySummaryDto
                    {
                        Id = g.Key,
                        Title = g.First().Title,
                        ProjectId = g.First().ProjectId,
                        ProjectName = g.First().ProjectName
                    };

                    if (!g.Any(p => p.Candidate is not null))
                    {
                        return hotVacancy;
                    }

                    var tempList = g.Where(p => p.Candidate is not null);

                    hotVacancy.CandidateCount = tempList.Count();
                    hotVacancy.ProcessedCount = tempList.Count(p => p.CurrentStageIndex == p.LastStageIndex);
                    hotVacancy.SelfAppliedCount = tempList.Count(p => p.IsSelfApplied == true);
                    hotVacancy.CandidateNewCount = tempList.Count(p => p.CurrentStageIndex == 0 || p.CurrentStageIndex == 1);

                    return hotVacancy;
                });

            return hotVacansiesDto.ToList();
        }
    }
}
