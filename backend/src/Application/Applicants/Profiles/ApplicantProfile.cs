using AutoMapper;
using Application.Applicants.Dtos;
using Domain.Entities;

namespace Application.Applicants.Profiles
{
    public class ApplicantProfile : Profile
    {
        public ApplicantProfile()
        {
            CreateMap<ApplicantDto, Applicant>();

            CreateMap<Applicant, ApplicantDto>()
                .ForMember(dto => dto.PhotoLink, opt => opt.MapFrom(a => a.PhotoFileInfo.PublicUrl));

            CreateMap<Applicant, GetShortApplicantDto>();
            CreateMap<Applicant, ApplicantCsvGetDto>();

            CreateMap<Applicant, MarkedApplicantDto>()
                .ForMember(dto => dto.IsApplied, opt => opt.Ignore());

            CreateMap<(Applicant, bool), MarkedApplicantDto>()
                .IncludeMembers(dto => dto, opt => opt.Item1)
                .ForMember(dto => dto.IsApplied, opt => opt
                         .MapFrom(ma => ma.Item2));

            CreateMap<CreateApplicantDto, ApplicantDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CreateApplicantDto, Applicant>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ApplicantCsvDto, Applicant>();
            CreateMap<ApplicantCsvDto, GetApplicantCsvDto>()
                .ForMember(dto => dto.IsValid, opt => opt.Ignore());

            CreateMap<UpdateApplicantDto, ApplicantDto>();
            CreateMap<UpdateApplicantDto, Applicant>();
            CreateMap<ApplicantVacancyInfo, ApplicantVacancyInfoDto>();
        }
    }
}