using AutoMapper;
using Application.ApplicantCvs.Dtos;
using Domain.Entities;

namespace Application.ApplicantCvs
{
    public class ApplicantCvProfile : Profile
    {
        public ApplicantCvProfile()
        {
            CreateMap<ApplicantCv, ApplicantCvDto>();
            CreateMap<ApplicantCvDto, ApplicantCv>();
        }
    }
}
