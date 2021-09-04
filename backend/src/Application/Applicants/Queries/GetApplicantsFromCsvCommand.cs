using Application.Applicants.Dtos;
using Application.Interfaces;
using AutoMapper;
using CsvHelper;
using Domain.Entities;
using Domain.Interfaces.Read;
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

namespace Application.Applicants.Queries
{
    public class GetApplicantsFromCsvCommand : IRequest<ICollection<GetApplicantCsvDto>>
    {
        public Stream Stream { get; set; }

        public GetApplicantsFromCsvCommand(Stream stream)
        {
            Stream = stream;
        }
    }

    public class GetApplicantsFromCsvCommandHandler : IRequestHandler<GetApplicantsFromCsvCommand, ICollection<GetApplicantCsvDto>>
    {
        protected readonly IApplicantsFromCsvWriteRepository _repository;
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IApplicantReadRepository _applicantReadRepository;
        private readonly IMapper _mapper;
        public GetApplicantsFromCsvCommandHandler(
            IApplicantsFromCsvWriteRepository repository, 
            ICurrentUserContext currentUserContext,
            IApplicantReadRepository applicantReadRepository,
            IMapper mapper)
        {
            _repository = repository;
            _currentUserContext = currentUserContext;
            _applicantReadRepository = applicantReadRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetApplicantCsvDto>> Handle(GetApplicantsFromCsvCommand command, CancellationToken _)
        {
            var applicantsEmails = (await _applicantReadRepository.GetEnumerableAsync()).Select(applicant=>applicant.Email);

            List<ApplicantCsvDto> applicantsDtos = new List<ApplicantCsvDto>();
            List<GetApplicantCsvDto> applicantsGetDtos = new List<GetApplicantCsvDto>();

            using (var streamReader = new StreamReader(command.Stream))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csvReader.Context.RegisterClassMap<ApplicantCsvDtoClassMap>();
                    applicantsDtos = csvReader.GetRecords<ApplicantCsvDto>().ToList();

                    var validator = new ApplicantCsvDtoValidator();

                    foreach(var applicantDto in applicantsDtos)
                    {
                        var applicantGetDto = _mapper.Map<GetApplicantCsvDto>(applicantDto);

                        if (validator.Validate(applicantDto).IsValid)
                        {
                            applicantGetDto.IsValid = true;
                        }

                        applicantsGetDtos.Add(applicantGetDto);
                    }
                }
            }

            return applicantsGetDtos;
        }

    }
}
