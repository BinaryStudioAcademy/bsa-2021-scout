using System;
using AutoMapper;
using MongoDB.Bson;
using Application.ApplicantCv.Dtos;
using entities = Domain.Entities;

namespace Application.ApplicantCv
{
    public class ApplicantCvProfile : Profile
    {
        public ApplicantCvProfile()
        {
            CreateMap<entities::ApplicantCv, ApplicantCvDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.ApplicantId, opt => opt.MapFrom(src => new Guid(src.ApplicantId)));

            CreateMap<ApplicantCvDto, entities::ApplicantCv>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ObjectId.Parse(src.Id)))
                .ForMember(dest => dest.ApplicantId, opt => opt.MapFrom(src => src.ApplicantId.ToString()));
        }
    }
}
