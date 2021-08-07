using AutoMapper;
using Domain.Entities;
using Application.Stages.Dtos;

namespace Application.Stages
{
    public class StageProfile : Profile
    {
        public StageProfile()
        {
            CreateMap<Stage, StageWithCandidatesDto>();
        }
    }
}
