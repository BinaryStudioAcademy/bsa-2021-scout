using Application.ApplicantCsvs.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.ApplicantCsvs
{
    public class ApplicantCsvProfile : Profile
    {
        public ApplicantCsvProfile()
        {
            CreateMap<CsvFile, CsvFileDto>();
            CreateMap<CsvFileDto, CsvFile>();
        }
    }
}
