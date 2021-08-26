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
                        .Select(g => new HotVacancySummaryDto
                        {
                            Id = g.Key,
                            Title = g.Select(p => p.Title).First(),
                            ProjectName = g.Select(p => p.ProjectName).FirstOrDefault(),
                            CandidateCount = g.Count(),
                            ProcessedCount = g.Where(p => p.CurrentStageIndex == p.LastStageIndex).Count(),
                            SelfAppliedCount = g.Where(p => p.HrWhoAddedId is null).Count(),
                            CandidateNewCount = g.Where(p => p.CurrentStageIndex == 0).Count()
                        });
            return hotVacansiesDto.ToList();
        }
    }
}
