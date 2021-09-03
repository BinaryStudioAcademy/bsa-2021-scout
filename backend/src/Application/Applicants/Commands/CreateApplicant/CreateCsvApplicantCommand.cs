using Application.Applicants.Dtos;
using Application.Common.Commands;
using Application.Common.Files.Dtos;
using Application.ElasticEnities.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Write;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Applicants.Commands.CreateApplicant
{
    public class CreateCsvApplicantCommand : IRequest<ApplicantCsvGetDto>
    {
        public CreateApplicantDto ApplicantDto { get; set; }
        public FileDto? CvFileDto { get; set; }

        public CreateCsvApplicantCommand(CreateApplicantDto applicantDto, FileDto? cvFileDto)
        {
            ApplicantDto = applicantDto;
            CvFileDto = cvFileDto;
        }
    }

    public class CreateCsvApplicantCommandHandler : IRequestHandler<CreateCsvApplicantCommand, ApplicantCsvGetDto>
    {
        private readonly IWriteRepository<Applicant> _applicantWriteRepository;
        private readonly IApplicantCvFileWriteRepository _applicantCvFileWriteRepository;
        protected readonly ICurrentUserContext _currentUserContext;
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public CreateCsvApplicantCommandHandler(
            IWriteRepository<Applicant> applicantWriteRepository,
            IApplicantCvFileWriteRepository applicantCvFileWriteRepository,
            ICurrentUserContext currentUserContext,
            IMapper mapper,
            ISender mediator
        )
        {
            _applicantWriteRepository = applicantWriteRepository;
            _applicantCvFileWriteRepository = applicantCvFileWriteRepository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ApplicantCsvGetDto> Handle(CreateCsvApplicantCommand command, CancellationToken _)
        {
            var creatorUser = await _currentUserContext.GetCurrentUser();

            var applicant = new Applicant
            {
                FirstName = command.ApplicantDto.FirstName,
                LastName = command.ApplicantDto.LastName,
                BirthDate = command.ApplicantDto.BirthDate,
                Email = command.ApplicantDto.Email,
                Phone = command.ApplicantDto.Phone,
                Experience = command.ApplicantDto.Experience,
                LinkedInUrl = command.ApplicantDto.LinkedInUrl,
                ExperienceDescription = command.ApplicantDto.ExperienceDescription,
                Skills = command.ApplicantDto.Skills,
                CompanyId = creatorUser.CompanyId,
                CreationDate = DateTime.Now
            };

            await UploadCvFileIfExists(applicant, command);

            await _applicantWriteRepository.CreateAsync(applicant);

            var createdApplicant = _mapper.Map<ApplicantCsvGetDto>(applicant);

            createdApplicant.User = creatorUser;

            return createdApplicant;
        }

        private async Task UploadCvFileIfExists(Applicant applicant, CreateCsvApplicantCommand command)
        {
            if (command.CvFileDto == null)
            {
                return;
            }

            var uploadedCvFileInfo = await _applicantCvFileWriteRepository.UploadAsync(applicant.Id, command.CvFileDto!.Content);
            applicant.CvFileInfo = uploadedCvFileInfo;
        }
    }
}
