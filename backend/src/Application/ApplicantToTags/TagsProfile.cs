using AutoMapper;
using Domain.Entities;
using Application.ApplicantToTags.Dtos;

namespace Application.ApplicantCv
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
