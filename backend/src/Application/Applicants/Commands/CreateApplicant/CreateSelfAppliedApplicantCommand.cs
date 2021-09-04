using Application.Applicants.Dtos;
using Application.Common.Commands;
using Application.Common.Files.Dtos;
using Application.ElasticEnities.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Write;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Applicants.Commands.CreateApplicant
{
    public class CreateSelfAppliedApplicantCommand : IRequest<ApplicantDto>
    {
        public CreateApplicantDto ApplicantDto { get; set; }
        public FileDto CvFileDto { get; set; }
        public string VacancyId { get; set; }

        public CreateSelfAppliedApplicantCommand(CreateApplicantDto applicantDto, FileDto cvFileDto, string vacancyId)
        {
            ApplicantDto = applicantDto;
            CvFileDto = cvFileDto;
            VacancyId = vacancyId;
        }
    }

    public class CreateSelfAppliedApplicantCommandHandler : IRequestHandler<CreateSelfAppliedApplicantCommand, ApplicantDto>
    {
        private readonly IApplicantsWriteRepository _applicantWriteRepository;
        private readonly IReadRepository<Vacancy> _vacancyReadRepository;
        private readonly IApplicantCvFileWriteRepository _applicantCvFileWriteRepository;
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public CreateSelfAppliedApplicantCommandHandler(
            IApplicantsWriteRepository applicantWriteRepository,
            IApplicantCvFileWriteRepository applicantCvFileWriteRepository,
            IReadRepository<Vacancy> vacancyReadRepository,
            IMapper mapper,
            ISender mediator
        )
        {
            _applicantWriteRepository = applicantWriteRepository;
            _applicantCvFileWriteRepository = applicantCvFileWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<ApplicantDto> Handle(CreateSelfAppliedApplicantCommand command, CancellationToken _)
        {
            var vacancy = await _vacancyReadRepository.GetAsync(command.VacancyId);

            var applicant = new Applicant
            {
                FirstName = command.ApplicantDto.FirstName,
                LastName = command.ApplicantDto.LastName,
                BirthDate = command.ApplicantDto.BirthDate,
                Email = command.ApplicantDto.Email,
                Phone = command.ApplicantDto.Phone,
                Experience = command.ApplicantDto.Experience,
                ExperienceDescription = command.ApplicantDto.ExperienceDescription,
                LinkedInUrl = command.ApplicantDto.LinkedInUrl,
                CompanyId = vacancy.CompanyId,
                IsSelfApplied = true,
                CreationDate = DateTime.Now
            };

            await UploadCvFileIfExists(applicant, command);

            await _applicantWriteRepository.CreateFullAsync(applicant);

            var createdApplicant = _mapper.Map<Applicant, ApplicantDto>(applicant);

            await CreateElasticEntityAndAddTagsIfExist(createdApplicant, command);

            createdApplicant.Vacancies = new List<ApplicantVacancyInfoDto>();

            return createdApplicant;
        }

        private async Task UploadCvFileIfExists(Applicant applicant, CreateSelfAppliedApplicantCommand command)
        {
            if (command.CvFileDto == null)
            {
                return;
            }

            var uploadedCvFileInfo = await _applicantCvFileWriteRepository.UploadAsync(applicant.Id, command.CvFileDto!.Content);
            applicant.CvFileInfo = uploadedCvFileInfo;
        }

        private async Task CreateElasticEntityAndAddTagsIfExist(ApplicantDto createdApplicant, CreateSelfAppliedApplicantCommand command)
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
