using AutoMapper;
using Domain.Entities;
using Application.VacancyCandidates.Dtos;

namespace Application.VacancyCandidates
{
    public class VacancyCandidateProfile : Profile
    {
        public VacancyCandidateProfile()
        {
            CreateMap<VacancyCandidate, VacancyCandidateDto>();
            CreateMap<VacancyCandidate, VacancyCandidateWithApplicantDto>();
        }
    }
}
