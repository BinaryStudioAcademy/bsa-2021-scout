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

            CreateMap<Applicant, PoolApplicantDto>()
                .ForMember(dto => dto.PhotoLink, opt => opt.MapFrom(a => a.PhotoFileInfo.PublicUrl));

            CreateMap<Pool, PoolDto>()
                .ForMember(dest => dest.Applicants, opt => opt.MapFrom(src => src.PoolApplicants.Select(p => p.Applicant)))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => $"{src.CreatedBy.FirstName} {src.CreatedBy.LastName}"));

            CreateMap<Pool, CreatePoolDto>();
        }
    }
}