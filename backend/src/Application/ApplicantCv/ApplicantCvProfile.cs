using AutoMapper;
using Application.ApplicantCv.Dtos;
using entities = Domain.Entities;

namespace Application.ApplicantCv
{
    public class ApplicantCvProfile : Profile
    {
        public ApplicantCvProfile()
        {
            CreateMap<entities::ApplicantCv, ApplicantCvDto>();
            CreateMap<ApplicantCvDto, entities::ApplicantCv>();
        }
    }
}
