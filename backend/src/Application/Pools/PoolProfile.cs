using AutoMapper;
using Domain.Entities;
using Application.Pools.Dtos;
using System.Linq;

namespace Application.Pools
{
    public class PoolProfile : Profile
    {
        public PoolProfile()
        {
            CreateMap<CreatePoolDto, Pool>();
            CreateMap<Applicant, PoolApplicantDto>();

            CreateMap<Pool, PoolDto>()
                .ForMember(dest => dest.Applicants, opt => opt.MapFrom(src => src.PoolApplicants.Select(p => p.Applicant)));                

            CreateMap<Pool, CreatePoolDto>();



        }
    }
}