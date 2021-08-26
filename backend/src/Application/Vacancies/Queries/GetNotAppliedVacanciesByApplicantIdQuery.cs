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
    public class GetNotAppliedVacanciesByApplicantIdQuery : IRequest<IEnumerable<ShortVacancyWithDepartmentDto>>
    {
        public string ApplicantId { get; }

        public GetNotAppliedVacanciesByApplicantIdQuery(string applicantId)
        {
            ApplicantId = applicantId;
        }
    }

    public class GetNotAppliedVacanciesByApplicantIdQueryHandler : IRequestHandler<GetNotAppliedVacanciesByApplicantIdQuery, IEnumerable<ShortVacancyWithDepartmentDto>>
    {
        protected readonly IVacancyReadRepository _repository;
        protected readonly IReadRepository<Project> _projectRepository;
        protected readonly IMapper _mapper;

        public GetNotAppliedVacanciesByApplicantIdQueryHandler(IVacancyReadRepository repository, IReadRepository<Project> projectRepository, IMapper mapper)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShortVacancyWithDepartmentDto>> Handle(GetNotAppliedVacanciesByApplicantIdQuery query, CancellationToken _)
        {
            var result = _mapper.Map<IEnumerable<ShortVacancyWithDepartmentDto>>(await _repository.GetEnumerableNotAppliedByApplicantId(query.ApplicantId));

            foreach (var dto in result)
            {
                dto.Department = (await _projectRepository.GetAsync(dto.ProjectId)).TeamInfo;
            }

            return result;
        }
    }
}
