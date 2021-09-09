using System;
using System.Linq;
using AutoMapper;
using Domain.Entities;
using Application.VacancyCandidates.Dtos;

namespace Application.VacancyCandidates
{
    public class VacancyCandidateProfile : Profile
    {
        public VacancyCandidateProfile()
        {
            CreateMap<VacancyCandidate, VacancyCandidateDto>();
            CreateMap<VacancyCandidateDto, VacancyCandidate>();

            CreateMap<VacancyCandidate, ShortVacancyCandidateWithApplicantDto>()
                .ForMember(
                    dto => dto.AverageMark,
                    opt => opt
                        .MapFrom(vc =>
                            vc.Reviews.Count > 0
                                ? Math.Max(Math.Min((int)Math.Ceiling(vc.Reviews.Select(r => r.Mark).Average()), 10), 0)
                                : (int?)null)
                );

            CreateMap<VacancyCandidate, VacancyCandidateFullDto>()
                .ForMember(
                    dto => dto.HrWhoAddedFullName,
                    opt => opt
                        .MapFrom(vc => vc.HrWhoAdded.FirstName + " " + vc.HrWhoAdded.LastName)
                )
                .ForMember(
                    dto => dto.CurrentStageName,
                    opt =>
                        opt.MapFrom(vc =>
                            vc.CandidateToStages.Count > 0
                                ? vc.CandidateToStages
                                    .Where(cts => cts.DateRemoved == null)
                                    .First()
                                    .Stage
                                    .Name
                                : null)
                )
                .ForMember(
                    dto => dto.FullName,
                    opt => opt
                        .MapFrom(vc => vc.Applicant.FirstName + " " + vc.Applicant.LastName)
                )
                .ForMember(dto => dto.StagesHistory, opt => opt.MapFrom(vc => vc.CandidateToStages))
                .ForMember(dto => dto.Email, opt => opt.MapFrom(vc => vc.Applicant.Email))
                .ForMember(dto => dto.Phone, opt => opt.MapFrom(vc => vc.Applicant.Phone))
                .ForMember(dto => dto.CvLink, opt =>
                    opt.MapFrom(vc => vc.Applicant.CvFileInfo == null ? null : vc.Applicant.CvFileInfo.PublicUrl))
                .ForMember(dto => dto.CvName, opt =>
                    opt.MapFrom(vc => vc.Applicant.CvFileInfo == null ? null : vc.Applicant.CvFileInfo.Name))
                .ForMember(dto => dto.Experience, opt => opt.MapFrom(vc => vc.Applicant.Experience))
                .ForMember(dto => dto.ExperienceDescription, opt =>
                    opt.MapFrom(vc => vc.Applicant.ExperienceDescription))
                .ForMember(dto => dto.PhotoLink, opt =>
                    opt.MapFrom(vc => vc.Applicant.PhotoFileInfo.PublicUrl));
        }
    }
}
