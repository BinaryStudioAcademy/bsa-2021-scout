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
                                ? vc.CandidateToStages.First().Stage.Name
                                : null)
                )
                .ForMember(
                    dto => dto.FullName,
                    opt => opt
                        .MapFrom(vc => vc.Applicant.FirstName + " " + vc.Applicant.LastName)
                )
                .ForMember(dto => dto.Email, opt => opt.MapFrom(vc => vc.Applicant.Email))
                .ForMember(dto => dto.Phone, opt => opt.MapFrom(vc => vc.Applicant.Phone))
                .ForMember(dto => dto.Cv, opt => opt.Ignore());
        }
    }
}
