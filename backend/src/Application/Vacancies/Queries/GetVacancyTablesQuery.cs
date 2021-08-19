using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Application.Vacancies.Dtos;
using System.Collections.Generic;
using Application.Common.Queries;
using Domain.Interfaces.Abstractions;
using MediatR;
using System.Linq;
using Application.Users.Dtos;
using Application.Interfaces;
using System;

namespace Application.Vacancies.Queries
{
    public class GetVacancyTablesListQuery : IRequest<IEnumerable<VacancyTableDto>>
    { }

    public class GetVacancyTablesQueryHandler : IRequestHandler<GetVacancyTablesListQuery, IEnumerable<VacancyTableDto>>
    {
        protected readonly IVacancyTableReadRepository _vacancyTableRepo;
        protected readonly ICurrentUserContext _context;

        protected readonly IMapper _mapper;

        public GetVacancyTablesQueryHandler(ICurrentUserContext context, IVacancyTableReadRepository vacancyTableRepo,
         IMapper mapper)
        {
            _vacancyTableRepo = vacancyTableRepo;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VacancyTableDto>> Handle(GetVacancyTablesListQuery query, CancellationToken _)
        {
            string companyId = "";

            companyId = (await _context.GetCurrentUser())?.CompanyId ?? "";
          
            IEnumerable<VacancyTable> result = await _vacancyTableRepo.GetVacancyTablesByCompanyIdAsync(companyId);
            IEnumerable<VacancyTableDto> dtos = _mapper.Map<IEnumerable<VacancyTableDto>>(result);
            //HERE!!!!
            // .ForMember(dest => dest.Department, opt => opt.MapFrom(v => v.Project.TeamInfo))
            // .ForMember(dest => dest.CurrentApplicantsAmount, opt => opt.MapFrom(
            //     v => v.Stages.Sum<Stage>(s => s.CandidateToStages.Count())))
            // .ForMember(dest => dest.RequiredCandidatesAmount, opt => opt.MapFrom(
            //     v => v.Stages.Sum<Stage>(s => s.Reviews.Count())));
            return dtos;
        }
    }
}
