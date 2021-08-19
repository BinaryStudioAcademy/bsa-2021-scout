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

            CreateMap<VacancyUpdateDto, Vacancy>();
            CreateMap<Vacancy, VacancyDto>();
            CreateMap<Vacancy, VacancyTableDto>();
            CreateMap<Vacancy, VacancyUpdateDto>();

            CreateMap<VacancyTable, VacancyTableDto>()
                .ForMember(dest => dest.TeamInfo, opt => opt.MapFrom(p => p.Project.TeamInfo));

            CreateMap<Vacancy, ShortVacancyWithStagesDto>();


            CreateMap<Vacancy, VacancyTableDto>();

        }
    }
}
