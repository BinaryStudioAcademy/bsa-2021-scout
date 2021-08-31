using Application.Applicants.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Write;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Applicants.Commands.CreateApplicant
{
    public class CreateRangeOfApplicantsCommand : IRequest<IEnumerable<ApplicantDto>>
    {
        public IEnumerable<CreateApplicantDto> ApplicantsDtos { get; set; }

        public CreateRangeOfApplicantsCommand(IEnumerable<CreateApplicantDto> applicantsDtos)
        {
            ApplicantsDtos = applicantsDtos;
        }
    }

    public class CreateRangeOfApplicantsCommandHandler : IRequestHandler<CreateRangeOfApplicantsCommand, IEnumerable<ApplicantDto>>
    {
        protected readonly IApplicantsFromCsvWriteRepository _repository;
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IMapper _mapper;

        public CreateRangeOfApplicantsCommandHandler(
            IApplicantsFromCsvWriteRepository repository,
            ICurrentUserContext currentUserContext,
            IMapper mapper)
        {
            _repository = repository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicantDto>> Handle(CreateRangeOfApplicantsCommand command, CancellationToken _)
        {
            var company = await _currentUserContext.GetCurrentUser();
            var companyId = company.CompanyId;

            var applicants = _mapper.Map<IEnumerable<Applicant>>(command.ApplicantsDtos);

            foreach (var applicant in applicants)
            {
                applicant.CompanyId = companyId;
            }

            var result = _mapper.Map<IEnumerable<ApplicantDto>>(await _repository.CreateRangeAsync(applicants));

            return result;
        }
    }
}
