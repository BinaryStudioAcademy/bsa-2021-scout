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
            CreateMap<ActionDto, Action>();
            CreateMap<Action, ActionDto>();
            CreateMap<ActionCreateDto, Action>();
            CreateMap<Action, ActionCreateDto>();
            CreateMap<StageWithCandidatesDto, Stage>();

            CreateMap<Stage, StageDto>()
                .ForMember(
                    dto => dto.Reviews,
                    opt => opt.MapFrom(s =>
                        s.ReviewToStages.Where(rts => rts.Review != null).Select(rts => rts.Review))
                );

            CreateMap<StageDto, Stage>()
                .ForMember(
                    s => s.ReviewToStages,
                    opt => opt.MapFrom(dto => dto.Reviews
                        .Select(r => new ReviewToStage { ReviewId = r.Id }))
                );

            CreateMap<Stage, StageCreateDto>()
                .ForMember(
                    dto => dto.Reviews,
                    opt => opt.MapFrom(s =>
                        s.ReviewToStages.Where(rts => rts.Review != null).Select(rts => rts.Review))
                );

            CreateMap<StageCreateDto, Stage>()
                .ForMember(
                    s => s.ReviewToStages,
                    opt => opt.MapFrom(scd => scd.Reviews
                        .Select(r => new ReviewToStage { ReviewId = r.Id, Review = new Review { Id = r.Id } }))
                )
                .ForMember(s => s.Reviews, opt => opt.Ignore());

            CreateMap<Stage, StageUpdateDto>()
                .ForMember(
                    dto => dto.Reviews,
                    opt => opt.MapFrom(s => s.ReviewToStages.Where(rts => rts.Review != null).Select(rts => rts.Review))
                );

            CreateMap<StageUpdateDto, Stage>()
                .ForMember(
                    s => s.ReviewToStages,
                    opt => opt.MapFrom((sud, s) => sud.Reviews
                            .Select(r =>
                                new ReviewToStage { StageId = s.Id, ReviewId = r.Id, Review = new Review { Id = r.Id } }))
                )
                .ForMember(s => s.Reviews, opt => opt.Ignore());

            CreateMap<Stage, StageWithCandidatesDto>()
                .ForMember(
                    dto => dto.Candidates,
                    opt => opt.MapFrom(s =>
                        s.CandidateToStages.Select(cts => cts.Candidate))
                )
                .ForMember(
                    dto => dto.Reviews,
                    opt => opt.MapFrom(s =>
                        s.ReviewToStages.Select(rts => rts.Review))
                );
        }
    }
}
