using AutoMapper;
using entities = Domain.Entities;
using Application.ElasticEnities.Dtos;
using Domain.Entities;

namespace Application.ElasticEnities
{
    public class ElasticEntitiesProfile : Profile
    {
        public ElasticEntitiesProfile()
        {
            CreateMap<entities::ElasticEntity, ElasticEnitityDto>()
            .ForMember(dest => dest.TagDtos, opt => opt.MapFrom(src => src.Tags));

            CreateMap<entities::ElasticEntity, CreateElasticEntityDto>()
           .ForMember(dest => dest.TagsDtos, opt => opt.MapFrom(src => src.Tags));

            CreateMap<entities::ElasticEntity, UpdateApplicantToTagsDto>()
           .ForMember(dest => dest.TagsDtos, opt => opt.MapFrom(src => src.Tags));

            CreateMap<ElasticEnitityDto, entities::ElasticEntity>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TagDtos));

            CreateMap<CreateElasticEntityDto, entities::ElasticEntity>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TagsDtos));

            CreateMap<UpdateApplicantToTagsDto, entities::ElasticEntity>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TagsDtos));

        }
    }
}
