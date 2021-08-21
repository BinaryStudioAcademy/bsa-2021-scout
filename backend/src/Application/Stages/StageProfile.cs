using System.Linq;
using AutoMapper;
using Domain.Entities;
using Application.Stages.Dtos;

namespace Application.Stages
{
    public class StageProfile : Profile
    {
        public StageProfile()
        {
            CreateMap<StageCreateDto, Stage>();
            CreateMap<StageUpdateDto, Stage>();
            CreateMap<Stage, StageUpdateDto>();
            CreateMap<Stage, StageCreateDto>();
            CreateMap<Stage, StageDto>();
            CreateMap<StageDto, Stage>();
            CreateMap<StageWithCandidatesDto, Stage>();

            CreateMap<Stage, StageWithCandidatesDto>()
                .ForMember(
                    dto => dto.Candidates,
                    opt => opt.MapFrom(s =>
                        s.CandidateToStages.Select(cts => cts.Candidate))
                );
        }
    }
}
