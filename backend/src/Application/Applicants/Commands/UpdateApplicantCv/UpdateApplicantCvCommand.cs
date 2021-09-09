using System.Linq;
using MediatR;
using Domain.Interfaces.Write;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Files.Dtos;
using Domain.Interfaces.Read;
using Domain.Interfaces.Abstractions;
using Domain.Entities;

#nullable enable
namespace Application.Applicants.Commands
{
    public class UpdateApplicantCvCommand : IRequest<Unit>
    {
        public string ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }
        public FileDto NewCvFileDto { get; set; }

        public UpdateApplicantCvCommand(string applicantId, FileDto cvFileDto, Applicant? applicant = null)
        {
            ApplicantId = applicantId;
            Applicant = applicant;
            NewCvFileDto = cvFileDto;
        }
    }

    public class UpdateApplicantCvCommandHandler : IRequestHandler<UpdateApplicantCvCommand, Unit>
    {
        private readonly IApplicantCvFileWriteRepository _applicantCvFileWriteRepository;
        private readonly IApplicantReadRepository _applicantReadRepository;
        private readonly IWriteRepository<Applicant> _applicantWriteRepository;
        private readonly IWriteRepository<FileInfo> _fileInfoWriteRepository;

        public UpdateApplicantCvCommandHandler(
            IApplicantCvFileWriteRepository applicantCvFileWriteRepository,
            IApplicantReadRepository applicantReadRepository,
            IWriteRepository<Applicant> applicantWriteRepository,
            IWriteRepository<FileInfo> fileInfoWriteRepository
        )
        {
            _applicantCvFileWriteRepository = applicantCvFileWriteRepository;
            _applicantReadRepository = applicantReadRepository;
            _applicantWriteRepository = applicantWriteRepository;
            _fileInfoWriteRepository = fileInfoWriteRepository;
        }

        public async Task<Unit> Handle(UpdateApplicantCvCommand command, CancellationToken _)
        {
            var applicant = command.Applicant ?? await _applicantReadRepository.GetByIdAsync(command.ApplicantId);

            if (applicant.HasCv)
            {
                await _applicantCvFileWriteRepository.UpdateAsync(command.ApplicantId, command.NewCvFileDto.Content);
            }
            else
            {
                FileInfo uploadedCvFileInfo;

                if (command.NewCvFileDto.Link == null)
                {
                    uploadedCvFileInfo = await _applicantCvFileWriteRepository
                        .UploadAsync(applicant.Id, command.NewCvFileDto!.Content);
                }
                else
                {
                    uploadedCvFileInfo = await _fileInfoWriteRepository
                        .CreateAsync(command.NewCvFileDto.ToFileInfo());
                }

                applicant.CvFileInfo = uploadedCvFileInfo;

                await _applicantWriteRepository.UpdateAsync(applicant);
            }

            return Unit.Value;
        }
    }
}
