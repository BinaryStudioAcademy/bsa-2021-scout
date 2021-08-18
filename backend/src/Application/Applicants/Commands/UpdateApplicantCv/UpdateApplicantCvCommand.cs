using MediatR;
using Domain.Interfaces.Write;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Files.Dtos;

namespace Application.Applicants.Commands
{
    public class UpdateApplicantCvCommand : IRequest<Unit>
    {
        public string ApplicantId { get; set; }
        public FileDto NewCvFileDto { get; set; }

        public UpdateApplicantCvCommand(string applicantId, FileDto cvFileDto)
        {
            ApplicantId = applicantId;
            NewCvFileDto = cvFileDto;
        }
    }

    public class UpdateApplicantCvCommandHandler : IRequestHandler<UpdateApplicantCvCommand, Unit>
    {
        private readonly IApplicantCvFileWriteRepository _applicantCvFileWriteRepository;

        public UpdateApplicantCvCommandHandler(IApplicantCvFileWriteRepository applicantCvFileWriteRepository)
        {
            _applicantCvFileWriteRepository = applicantCvFileWriteRepository;
        }

        public async Task<Unit> Handle(UpdateApplicantCvCommand command, CancellationToken _)
        {
            await _applicantCvFileWriteRepository.UpdateAsync(command.ApplicantId, command.NewCvFileDto.Content);

            return Unit.Value;
        }
    }
}
