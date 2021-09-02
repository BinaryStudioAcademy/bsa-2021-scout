using Application.Users.Dtos;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using Domain.Entities;
using AutoMapper;
using MediatR;
using Application.Applicants.Dtos;
using Domain.Interfaces.Write;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Files.Dtos;
using Application.Interfaces;
using Application.ElasticEnities.Dtos;
using System.Collections.Generic;
using System.Linq;
using System;

#nullable enable
namespace Application.Applicants.Commands
{
    public class CreateApplicantCommand : IRequest<ApplicantDto>
    {
        public CreateApplicantDto ApplicantDto { get; set; }
        public FileDto? CvFileDto { get; set; }

        public CreateApplicantCommand(CreateApplicantDto applicantDto, FileDto? cvFileDto)
        {
            ApplicantDto = applicantDto;
            CvFileDto = cvFileDto;
        }
    }

    public class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, ApplicantDto>
    {
        private readonly IWriteRepository<Applicant> _applicantWriteRepository;
        private readonly IApplicantCvFileWriteRepository _applicantCvFileWriteRepository;
        protected readonly ICurrentUserContext _currentUserContext;
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public CreateApplicantCommandHandler(
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

        public async Task<ApplicantDto> Handle(CreateApplicantCommand command, CancellationToken _)
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

            var createdApplicant = _mapper.Map<Applicant, ApplicantDto>(applicant);

            await CreateElasticEntityAndAddTagsIfExist(createdApplicant, command);

            createdApplicant.Vacancies = new List<ApplicantVacancyInfoDto>();

            return createdApplicant;
        }

        private async Task UploadCvFileIfExists(Applicant applicant, CreateApplicantCommand command)
        {
            if (command.CvFileDto == null)
            {
                return;
            }

            var uploadedCvFileInfo = await _applicantCvFileWriteRepository.UploadAsync(applicant.Id, command.CvFileDto!.Content);
            applicant.CvFileInfo = uploadedCvFileInfo;
        }

        private async Task CreateElasticEntityAndAddTagsIfExist(ApplicantDto createdApplicant, CreateApplicantCommand command)
        {
            var elasticEntityDto = new CreateElasticEntityDto()
            {
                ElasticType = ElasticType.ApplicantTags,
                Id = createdApplicant.Id,
                TagsDtos = new List<TagDto>()
            };

            if (command.ApplicantDto.Tags.TagDtos.Any())
            {
                elasticEntityDto.TagsDtos = command.ApplicantDto.Tags.TagDtos.Select(t => new TagDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    TagName = t.TagName
                });
            }

            var elasticQuery = new CreateElasticDocumentCommand<CreateElasticEntityDto>(elasticEntityDto);
            createdApplicant.Tags = _mapper.Map<ElasticEnitityDto>(await _mediator.Send(elasticQuery));
        }
    }
}
