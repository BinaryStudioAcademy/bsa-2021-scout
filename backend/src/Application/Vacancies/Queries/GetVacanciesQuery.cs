using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Application.Vacancies.Dtos;
using System.Collections.Generic;
using Application.Common.Queries;
using Domain.Interfaces.Abstractions;

namespace Application.Vacancies.Queries
{
    public class GetVacanciesQueryHandler
        : GetEntityListQueryHandler<Vacancy, VacancyDto>
    {
        public GetVacanciesQueryHandler(IReadRepository<Vacancy> repo, IMapper mapper): base(repo, mapper)
        { }
    }
}
