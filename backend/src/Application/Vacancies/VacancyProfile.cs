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
            CreateMap<VacancyCreateDto, Vacancy>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(x => VacancyStatus.Active));

            CreateMap<Vacancy, VacancyDto>();
            CreateMap<Vacancy, VacancyCreateDto>();
            CreateMap<Vacancy, ShortVacancyWithDepartmentDto>();
            CreateMap<Vacancy, VacancyTableDto>();
            CreateMap<VacancyUpdateDto, Vacancy>();
            CreateMap<Vacancy, VacancyDto>();
            CreateMap<Vacancy, VacancyTableDto>();
            CreateMap<Vacancy, VacancyUpdateDto>();
            CreateMap<VacancyTable, VacancyTableDto>();
            CreateMap<Vacancy, ShortVacancyWithStagesDto>();
            CreateMap<Vacancy, VacancyTableDto>();

            CreateMap<Vacancy, VacancyTable>();
        }
    }
}
