using Application.Vacancies.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Vacancies.Queries
{
    public class GetShortVacanciesWithDepartmentQuery : IRequest<IEnumerable<ShortVacancyWithDepartmentDto>>
    { }

    public class GetShortVacanciesWithDepartmentQueryHandler : IRequestHandler<GetShortVacanciesWithDepartmentQuery, IEnumerable<ShortVacancyWithDepartmentDto>>
    {
        protected readonly IVacancyReadRepository _repository;
        protected readonly IReadRepository<Project> _projectRepository;
        protected readonly IMapper _mapper;

        public GetShortVacanciesWithDepartmentQueryHandler(IVacancyReadRepository repository, IReadRepository<Project> projectRepository, IMapper mapper)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShortVacancyWithDepartmentDto>> Handle(GetShortVacanciesWithDepartmentQuery query, CancellationToken _)
        {
            var result = _mapper.Map<IEnumerable<ShortVacancyWithDepartmentDto>>(await _repository.GetEnumerableAsync());

            foreach (var dto in result)
            {
                dto.Department = (await _projectRepository.GetAsync(dto.ProjectId)).TeamInfo;
            }

            return result;
        }
    }
}
