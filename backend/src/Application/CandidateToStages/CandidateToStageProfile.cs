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

            CreateMap<CandidateToStage, CandidateToStageRecentActivityDto>()
                .ForMember(dto => dto.MoverId, opt => opt.MapFrom(cts => cts.Mover.Id))
                .ForMember(dto => dto.MoverName, opt =>
                    opt.MapFrom(cts => cts.Mover.FirstName + " " + cts.Mover.LastName))
                .ForMember(dto => dto.CandidateId, opt => opt.MapFrom(cts => cts.Candidate.Id))
                .ForMember(dto => dto.CandidateName, opt =>
                    opt.MapFrom(cts => cts.Candidate.Applicant.FirstName + " " + cts.Candidate.Applicant.LastName))
                .ForMember(dto => dto.StageId, opt => opt.MapFrom(cts => cts.Stage.Id))
                .ForMember(dto => dto.StageName, opt => opt.MapFrom(cts => cts.Stage.Name))
                .ForMember(dto => dto.VacancyId, opt => opt.MapFrom(cts => cts.Stage.Vacancy.Id))
                .ForMember(dto => dto.VacancyName, opt => opt.MapFrom(cts => cts.Stage.Vacancy.Title));

            CreateMap<CandidateToStage, CandidateToStageApplicantRecentActivityDto>()
                .ForMember(dto => dto.MoverId, opt => opt.MapFrom(cts => cts.Mover.Id))
                .ForMember(dto => dto.MoverName, opt =>
                    opt.MapFrom(cts => cts.Mover.FirstName + " " + cts.Mover.LastName))
                .ForMember(dto => dto.StageId, opt => opt.MapFrom(cts => cts.Stage.Id))
                .ForMember(dto => dto.StageName, opt => opt.MapFrom(cts => cts.Stage.Name));
        }
    }
}
