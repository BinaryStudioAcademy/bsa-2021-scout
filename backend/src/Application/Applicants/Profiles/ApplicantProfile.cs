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
            CreateMap<Applicant, ApplicantDto>();
            CreateMap<CreateApplicantDto, ApplicantDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ApplicantCsvDto, Applicant>();

            CreateMap<UpdateApplicantDto, ApplicantDto>();
            CreateMap<ApplicantVacancyInfo, ApplicantVacancyInfoDto>();
        }
    }
}