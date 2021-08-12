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

namespace Application.Applicants.Commands.Create
{
    public class CreateApplicantCommand : IRequest<ApplicantDto>
    {
        public CreateApplicantDto ApplicantDto { get; set; }
        public FileDto CvFileDto { get; set; }

        public CreateApplicantCommand(CreateApplicantDto applicantDto, FileDto cvFileDto)
        {
            ApplicantDto = applicantDto;
            CvFileDto = cvFileDto;
        }
    }

    public class CreateApplicantCommandHandler : IRequestHandler<CreateApplicantCommand, ApplicantDto>
    {
        private readonly IWriteRepository<Applicant> _applicantWriteRepository;
        private readonly IApplicantCvFileWriteRepository _applicantCvFileWriteRepository;
        private readonly IMapper _mapper;

        public CreateApplicantCommandHandler(
            IWriteRepository<Applicant> applicantWriteRepository,
            IApplicantCvFileWriteRepository applicantCvFileWriteRepository,
            IMapper mapper
        )
        {
            _applicantWriteRepository = applicantWriteRepository;
            _applicantCvFileWriteRepository = applicantCvFileWriteRepository;
            _mapper = mapper;
        }

        public async Task<ApplicantDto> Handle(CreateApplicantCommand command, CancellationToken _)
        {
            var applicant = new Applicant
            {
                FirstName = "",
                LastName = "",
                MiddleName = "",
                BirthDate = System.DateTime.Now,
                Email = "",
                Phone = command.ApplicantDto.Phone,
                Skype = command.ApplicantDto.Skype,
                Experience = command.ApplicantDto.Experience,
                ToBeContacted = command.ApplicantDto.ToBeContacted,
                CompanyId = command.ApplicantDto.CompanyId,
            };

            var uploadedCvFileInfo = await _applicantCvFileWriteRepository.UploadAsync(applicant.Id, command.CvFileDto.Content);

            applicant.CvFileInfo = uploadedCvFileInfo;

            await _applicantWriteRepository.CreateAsync(applicant);

            return _mapper.Map<Applicant, ApplicantDto>(applicant);
        }
    }
}
