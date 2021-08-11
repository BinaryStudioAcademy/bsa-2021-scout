using AutoMapper;
using Domain.Entities;
using Application.Vacancies.Dtos;
using Domain.Enums;
using System.Linq;
namespace Application.Vacancies
{
    public class VacancyProfile : Profile
    {
        public VacancyProfile()
        {
            //CreateMap<VacancyDto, Vacancy>();
            CreateMap<VacancyCreateDto, Vacancy>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(x => VacancyStatus.Active));
            //.ForMember(dest=>dest.Company, opt=>opt.);
            CreateMap<Vacancy, VacancyDto>();
            CreateMap<Vacancy, VacancyCreateDto>();
            
            CreateMap<Vacancy, VacancyTableDto>()
            .ForMember(dest => dest.Department, opt => opt.MapFrom(v => v.Project.TeamInfo))
            .ForMember(dest => dest.CurrentApplicantsAmount, opt => opt.MapFrom(
                v => v.Stages.Sum<Stage>(s => s.CandidateToStages.Count())))
            .ForMember(dest => dest.RequiredCandidatesAmount, opt => opt.MapFrom(
                v => v.Stages.Sum<Stage>(s => s.Reviews.Count())));

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
