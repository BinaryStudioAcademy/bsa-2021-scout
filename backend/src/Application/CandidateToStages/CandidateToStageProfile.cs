using AutoMapper;
using Domain.Entities;
using Application.CandidateToStages.Dtos;

namespace Application.CandidateToStages
{
    public class CandidateToStageProfile : Profile
    {
        public CandidateToStageProfile()
        {
            CreateMap<CandidateToStage, CandidateToStageHistoryDto>();
        }
    }
}
