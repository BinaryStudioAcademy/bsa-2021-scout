using Application.Applicants.Dtos;
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
        private readonly IMapper _mapper;
        public CreateApplicantsFromCsvCommandHandler(IApplicantsFromCsvWriteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<ApplicantCsvDto>> Handle(CreateApplicantsFromCsvCommand command, CancellationToken _)
        {
            List<ApplicantCsvDto> applicants = new List<ApplicantCsvDto>();

            using (var streamReader = new StreamReader(command.Stream))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csvReader.Context.RegisterClassMap<ApplicantCsvDtoClassMap>();
                    applicants = csvReader.GetRecords<ApplicantCsvDto>().ToList();

                    var validator = new ApplicantCsvDtoValidator();

                    applicants.RemoveAll(applicant => !validator.Validate(applicant).IsValid);

                    await _repository.CreateRangeAsync(_mapper.Map<ICollection<Applicant>>(applicants));
                }
            }

            return applicants;
        }

    }
}
