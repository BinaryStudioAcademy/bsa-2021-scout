using Application.Applicants.Dtos;
using Application.Interfaces;
using AutoMapper;
using CsvHelper;
using Domain.Entities;
using Domain.Interfaces.Write;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Applicants.Commands
{
    public class CreateApplicantsFromCsvCommand : IRequest<ICollection<ApplicantCsvDto>>
    {
        public Stream Stream { get; set; }

        public CreateApplicantsFromCsvCommand(Stream stream)
        {
            Stream = stream;
        }
    }

    public class CreateApplicantsFromCsvCommandHandler : IRequestHandler<CreateApplicantsFromCsvCommand, ICollection<ApplicantCsvDto>>
    {
        protected readonly IApplicantsFromCsvWriteRepository _repository;
        protected readonly ICurrentUserContext _currentUserContext;
        private readonly IMapper _mapper;
        public CreateApplicantsFromCsvCommandHandler(IApplicantsFromCsvWriteRepository repository, ICurrentUserContext currentUserContext, IMapper mapper)
        {
            _repository = repository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        public async Task<ICollection<ApplicantCsvDto>> Handle(CreateApplicantsFromCsvCommand command, CancellationToken _)
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            List<ApplicantCsvDto> applicantsDtos = new List<ApplicantCsvDto>();

            using (var streamReader = new StreamReader(command.Stream))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csvReader.Context.RegisterClassMap<ApplicantCsvDtoClassMap>();
                    applicantsDtos = csvReader.GetRecords<ApplicantCsvDto>().ToList();

                    var validator = new ApplicantCsvDtoValidator();

                    applicantsDtos.RemoveAll(applicant => !validator.Validate(applicant).IsValid);

                    var applicants = _mapper.Map<ICollection<Applicant>>(applicantsDtos);

                    foreach (var applicant in applicants)
                    {
                        applicant.CompanyId = companyId;
                    }

                    await _repository.CreateRangeAsync(applicants);
                }
            }

            return applicantsDtos;
        }

    }
}
