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
    public class UpdateApplicantPhotoCommand : IRequest<Unit>
    {
        public string ApplicantId { get; set; }
        public Applicant? Applicant { get; set; }
        public FileDto NewPhotoFileDto { get; set; }

        public UpdateApplicantPhotoCommand(string applicantId, FileDto photoFileDto, Applicant? applicant = null)
        {
            ApplicantId = applicantId;
            Applicant = applicant;
            NewPhotoFileDto = photoFileDto;
        }
    }

    public class UpdateApplicantPhotoCommandHandler : IRequestHandler<UpdateApplicantPhotoCommand, Unit>
    {
        private readonly IApplicantPhotoFileWriteRepository _applicantPhotoFileWriteRepository;
        private readonly IApplicantReadRepository _applicantReadRepository;
        private readonly IWriteRepository<Applicant> _applicantWriteRepository;
        private readonly IWriteRepository<FileInfo> _fileInfoWriteRepository;

        public UpdateApplicantPhotoCommandHandler(
            IApplicantPhotoFileWriteRepository applicantPhotoFileWriteRepository,
            IApplicantReadRepository applicantReadRepository,
            IWriteRepository<Applicant> applicantWriteRepository,
            IWriteRepository<FileInfo> fileInfoWriteRepository
        )
        {
            _applicantPhotoFileWriteRepository = applicantPhotoFileWriteRepository;
            _applicantReadRepository = applicantReadRepository;
            _applicantWriteRepository = applicantWriteRepository;
            _fileInfoWriteRepository = fileInfoWriteRepository;
        }

        public async Task<Unit> Handle(UpdateApplicantPhotoCommand command, CancellationToken _)
        {
            var applicant = command.Applicant ?? await _applicantReadRepository.GetByIdAsync(command.ApplicantId);

            if (applicant.HasPhoto)
            {
                await _applicantPhotoFileWriteRepository
                    .UpdateAsync(command.ApplicantId, command.NewPhotoFileDto.Content);
            }
            else
            {
                FileInfo uploadedPhotoFileInfo;

                if (command.NewPhotoFileDto.Link == null)
                {
                    uploadedPhotoFileInfo = await _applicantPhotoFileWriteRepository
                        .UploadAsync(applicant.Id, command.NewPhotoFileDto!.Content);
                }
                else
                {
                    uploadedPhotoFileInfo = await _fileInfoWriteRepository
                        .CreateAsync(command.NewPhotoFileDto.ToFileInfo());
                }

                applicant.CvFileInfo = uploadedPhotoFileInfo;

                await _applicantWriteRepository.UpdateAsync(applicant);
            }

            return Unit.Value;
        }
    }
}
