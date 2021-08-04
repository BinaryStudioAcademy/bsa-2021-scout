using AutoMapper;
using Domain.Entities;
using Application.Applicants.Dtos;

namespace Application.Applicants
{
    public class ApplicantProfile : Profile
    {
        public ApplicantProfile()
        {
            CreateMap<Applicant, ApplicantDto>();
        }
    }
}
