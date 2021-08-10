using AutoMapper;
using Domain.Entities;
using Application.ApplicantToTags.Dtos;

namespace Application.ApplicantToTags
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
