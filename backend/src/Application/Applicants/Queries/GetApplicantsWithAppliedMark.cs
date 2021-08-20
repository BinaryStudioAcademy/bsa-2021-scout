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

    public class GetApplicantsWithAppliedMark : IRequest<IEnumerable<MarkedApplicantDto>>
    {
        public string VacancyId { get; private set; }

        public GetApplicantsWithAppliedMark(string vacancyId)
        {
            VacancyId = vacancyId;
        }
    }

    public class GetApplicantsWithAppliedMarkHandler : IRequestHandler<GetApplicantsWithAppliedMark, IEnumerable<MarkedApplicantDto>>
    {
        private readonly IApplicantReadRepository _repository;
        protected readonly IMapper _mapper;

        public GetApplicantsWithAppliedMarkHandler(IApplicantReadRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MarkedApplicantDto>> Handle(GetApplicantsWithAppliedMark query, CancellationToken _)
        {
            var applicantsWithMarks = _mapper.Map<IEnumerable<MarkedApplicantDto>>(await _repository.GetApplicantsWithAppliedMark(query.VacancyId));

            return applicantsWithMarks;
        }
    }
}
