using AutoMapper;
using Domain.Entities;
using Application.Vacancies.Dtos;
using Domain.Enums;

namespace Application.Vacancies
{
    public class VacancyProfile : Profile
    {
        public VacancyProfile()
        {
            //CreateMap<VacancyDto, Vacancy>();
            CreateMap<VacancyCreateDto, Vacancy>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(x => VacancyStatus.Ok));
                //.ForMember(dest=>dest.Company, opt=>opt.);
            CreateMap<Vacancy, VacancyDto>();
            CreateMap<Vacancy, VacancyCreateDto>();

            CreateMap<VacancyUpdateDto, Vacancy>();
            CreateMap<Vacancy, VacancyDto>();
            CreateMap<Vacancy, VacancyUpdateDto>();

            CreateMap<Vacancy, ShortVacancyWithStagesDto>();
            //+update

            //CreateMap<User, UserDto>()
            //    .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(p => p.Role)));

            //CreateMap<UserDto, User>()
            //    .ForMember(dest => dest.DomainEvents, opt => opt.Ignore())
            //    .ForMember(dest => dest.UserRoles, opt => opt.MapFrom((src, dest, i, context) =>
            //        src.Roles.Select(role => new UserToRole()
            //        {
            //            RoleId = role.Id
            //        })));
        }
    }
}
