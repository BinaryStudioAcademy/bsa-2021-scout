using System;
using AutoMapper;
using entities = Domain.Entities;
using Application.ElasticEnities.Dtos;
using System.Collections.Generic;
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

            CreateMap<CreateElasticEntityDto, ElasticEnitityDto>()
                .ForMember(dest => dest.TagDtos, opt => opt.MapFrom(src => src.TagsDtos == null
                    ? new List<TagDto>()
                    : src.TagsDtos));

            CreateMap<UpdateApplicantToTagsDto, entities::ElasticEntity>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TagsDtos));

            CreateMap<ElasticEnitityDto, UpdateApplicantToTagsDto>()
                .ForMember(dest => dest.TagsDtos, opt => opt.MapFrom(src => src.TagDtos == null
                    ? new List<TagDto>()
                    : src.TagDtos));

            CreateMap<UpdateApplicantToTagsDto, ElasticEnitityDto>()
                .ForMember(dest => dest.TagDtos, opt => opt.MapFrom(src => src.TagsDtos == null
                    ? new List<TagDto>()
                    : src.TagsDtos));
        }
    }
}
