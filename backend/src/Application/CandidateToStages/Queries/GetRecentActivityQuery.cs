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
    public class GetRecentActivityQuery : IRequest<IEnumerable<CandidateToStageRecentActivityDto>>
    {
        public string UserId { get; set; }
        public int Page { get; set; }

        public GetRecentActivityQuery(string userId, int page = 1)
        {
            UserId = userId;
            Page = page;
        }
    }

    public class GetRecentActivityQueryHandler
        : IRequestHandler<GetRecentActivityQuery, IEnumerable<CandidateToStageRecentActivityDto>>
    {
        private readonly ICandidateToStageReadRepository _repository;
        private readonly IMapper _mapper;

        public GetRecentActivityQueryHandler(ICandidateToStageReadRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CandidateToStageRecentActivityDto>> Handle(
            GetRecentActivityQuery query,
            CancellationToken _
        )
        {
            IEnumerable<CandidateToStage> candidateToStages =
                await _repository.GetRecentAsync(query.UserId, query.Page);

            IEnumerable<CandidateToStageRecentActivityDto> dtos = _mapper
                .Map<IEnumerable<CandidateToStage>, IEnumerable<CandidateToStageRecentActivityDto>>(
                    candidateToStages
                );

            return dtos;
        }
    }
}
