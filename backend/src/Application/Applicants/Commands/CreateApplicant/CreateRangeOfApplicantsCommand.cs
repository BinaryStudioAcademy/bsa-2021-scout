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
    public class CreateRangeOfApplicantsCommand : IRequest<IEnumerable<ApplicantCsvGetDto>>
    {
        public IEnumerable<CreateApplicantDto> ApplicantsDtos { get; set; }

        public CreateRangeOfApplicantsCommand(IEnumerable<CreateApplicantDto> applicantsDtos)
        {
            ApplicantsDtos = applicantsDtos;
        }
    }

    public class CreateRangeOfApplicantsCommandHandler : IRequestHandler<CreateRangeOfApplicantsCommand, IEnumerable<ApplicantCsvGetDto>>
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

        public async Task<IEnumerable<ApplicantCsvGetDto>> Handle(CreateRangeOfApplicantsCommand command, CancellationToken _)
        {
            var user = await _currentUserContext.GetCurrentUser();
            var companyId = user.CompanyId;

            var applicants = _mapper.Map<IEnumerable<Applicant>>(command.ApplicantsDtos);

            foreach (var applicant in applicants)
            {
                applicant.CompanyId = companyId;
                applicant.CreationDate = DateTime.Now;
            }

            var result = await _repository.CreateRangeAsync(applicants);

            var createdApplicants = _mapper.Map<IEnumerable<ApplicantCsvGetDto>>(result);

            foreach (var applicant in createdApplicants)
            {
                applicant.User = user;
            }
            return createdApplicants;
        }
    }
}
