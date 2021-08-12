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

namespace Application.Vacancies.Queries
{
    public class GetVacancyTablesListQuery : IRequest<IEnumerable<VacancyTableDto>>
    { }

    public class GetVacancyTablesQueryHandler : IRequestHandler<GetVacancyTablesListQuery, IEnumerable<VacancyTableDto>>
    {
        protected readonly IReadRepository<Vacancy> _vacancyRepo;
        protected readonly IReadRepository<Project> _projectRepo;
        protected readonly IReadRepository<User> _userRepo;
        protected readonly IMapper _mapper;

        public GetVacancyTablesQueryHandler(IReadRepository<Vacancy> vacancyRepo,
         IReadRepository<Project> projectRepo, IReadRepository<User> userRepo,
         IMapper mapper)
        {
            _vacancyRepo = vacancyRepo;
            _projectRepo = projectRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VacancyTableDto>> Handle(GetVacancyTablesListQuery query, CancellationToken _)
        {
            IEnumerable<Vacancy> result = await _vacancyRepo.GetEnumerableAsync();
            IEnumerable<VacancyTableDto> dtos = _mapper.Map<IEnumerable<VacancyTableDto>>(result);
            foreach (var dto in dtos){
                dto.Department = (await _projectRepo.GetAsync(dto.ProjectId)).TeamInfo;
                dto.ResponsibleHr =  _mapper.Map<UserDto>((await _userRepo.GetAsync(dto.ResponsibleHrId)));
            }

            // .ForMember(dest => dest.Department, opt => opt.MapFrom(v => v.Project.TeamInfo))
            // .ForMember(dest => dest.CurrentApplicantsAmount, opt => opt.MapFrom(
            //     v => v.Stages.Sum<Stage>(s => s.CandidateToStages.Count())))
            // .ForMember(dest => dest.RequiredCandidatesAmount, opt => opt.MapFrom(
            //     v => v.Stages.Sum<Stage>(s => s.Reviews.Count())));
            return dtos;
        }
    }
}
