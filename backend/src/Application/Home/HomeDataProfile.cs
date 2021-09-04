using Application.Home.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.HomeData;

namespace Application.Home
{
    public class HomeDataProfile : Profile
    {
        public HomeDataProfile()
        {
            CreateMap<Vacancy, ShortVacancyDto>();
            CreateMap<WidgetsData, WidgetsDataDto>();
        }
    }
}
