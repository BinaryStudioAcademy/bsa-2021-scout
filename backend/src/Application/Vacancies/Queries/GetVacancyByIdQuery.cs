using Application.Common.Queries;
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
    public class GetVacancyByIdQuery : IRequest<VacancyDto>
    {
        public string Id { get; }

        public GetVacancyByIdQuery(string id)
        {
            Id = id;
        }
    }

    public class GetVacancyByIdQueryHandler : IRequestHandler<GetVacancyByIdQuery, VacancyDto>
    {
        protected readonly IVacancyReadRepository _repository;
        protected readonly IMapper _mapper;

        public GetVacancyByIdQueryHandler(IVacancyReadRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<VacancyDto> Handle(GetVacancyByIdQuery query, CancellationToken _)
        {
            var result = await _repository.GetByCompanyIdAsync(query.Id);

            return _mapper.Map<VacancyDto>(result);
        }
    }
}
