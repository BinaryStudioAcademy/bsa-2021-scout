using AutoMapper;
using Domain.Entities;
using Application.ElasticEnities.Dtos;

namespace Application.ElasticEnities
{
    public class TagsProfile : Profile
    {
        public TagsProfile()
        {
            CreateMap<Tag, TagDto>();
            CreateMap<TagDto, Tag>();
        }
    }
}
