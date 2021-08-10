using AutoMapper;
using entities = Domain.Entities;
using Application.ApplicantToTags.Dtos;
using Domain.Entities;

namespace Application.ApplicantToTags
{
    public class ApplicantToTagProfile : Profile
    {
        public ApplicantToTagProfile()
        {
            CreateMap<entities::ApplicantToTags, ApplicantToTagsDto>()
            .ForMember(dest => dest.TagDtos, opt => opt.MapFrom(src => src.Tags));

            CreateMap<entities::ApplicantToTags, CreateApplicantToTagsDto>()
           .ForMember(dest => dest.TagsDtos, opt => opt.MapFrom(src => src.Tags));

            CreateMap<entities::ApplicantToTags, UpdateApplicantToTagsDto>()
           .ForMember(dest => dest.TagsDtos, opt => opt.MapFrom(src => src.Tags));

            CreateMap<ApplicantToTagsDto, entities::ApplicantToTags>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TagDtos));

            CreateMap<CreateApplicantToTagsDto, entities::ApplicantToTags>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TagsDtos));

            CreateMap<UpdateApplicantToTagsDto, entities::ApplicantToTags>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TagsDtos));

        }
    }
}
