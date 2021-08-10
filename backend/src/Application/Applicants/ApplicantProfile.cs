using AutoMapper;
using Application.Applicants.Dtos;
using Domain.Entities;

namespace Application.Applicants
{
    public class ApplicantProfile : Profile
    {
        public ApplicantProfile()
        {
            CreateMap<ApplicantDto, Applicant>();
            CreateMap<Applicant, ApplicantDto>();
            CreateMap<CreateApplicantDto, Applicant>();
            CreateMap<UpdateApplicantDto, Applicant>();
        }
    }
}