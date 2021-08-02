using AutoMapper;
using entities = Domain.Entities;
using Application.ApplicantToTags.Dtos;

namespace Application.ApplicantCv
{
    public class ApplicantToTagProfile : Profile
    {
        public ApplicantToTagProfile()
        {
            CreateMap<entities::ApplicantToTags, ApplicantToTagsDto>();
            CreateMap<ApplicantToTagsDto, entities::ApplicantToTags>();
        }
    }
}
