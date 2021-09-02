using Application.SelfApply.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.SelfApply
{
    public class SelfApplyProfile : Profile
    {
        public SelfApplyProfile()
        {
            CreateMap<ApplyTokenDto, ApplyToken>();
            CreateMap<ApplyToken, ApplyTokenDto>();
        }
    }

}
