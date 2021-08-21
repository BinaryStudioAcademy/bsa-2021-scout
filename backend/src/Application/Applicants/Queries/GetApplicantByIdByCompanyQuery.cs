using Application.Applicants.Dtos;
using AutoMapper;
using Domain.Interfaces.Read;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Applicants.Queries
{
    public class GetApplicantByIdByCompanyQuery : IRequest<GetShortApplicantDto>
    {
        public string Id { get; }

        public GetApplicantByIdByCompanyQuery(string id)
        {
            Id = id;
        }
    }

    public class GetVacancyByIdQueryHandler : IRequestHandler<GetApplicantByIdByCompanyQuery, GetShortApplicantDto>
    {
        protected readonly IApplicantReadRepository _repository;
        protected readonly IMapper _mapper;

        public GetVacancyByIdQueryHandler(IApplicantReadRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetShortApplicantDto> Handle(GetApplicantByIdByCompanyQuery query, CancellationToken _)
        {
            var result = await _repository.GetByCompanyIdAsync(query.Id);

            return _mapper.Map<GetShortApplicantDto>(result);
        }
    }
}
