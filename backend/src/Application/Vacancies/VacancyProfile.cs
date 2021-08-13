using AutoMapper;
using Domain.Entities;
using Application.Vacancies.Dtos;

namespace Application.Vacancies
{
    public class VacancyProfile : Profile
    {
        public VacancyProfile()
        {
            CreateMap<Vacancy, ShortVacancyWithStagesDto>();

            CreateMap<Vacancy, VacancyTableDto>();
        }
    }
}
