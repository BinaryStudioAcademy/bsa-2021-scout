using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Users.Dtos;
using Application.Interfaces;
using Application.Interfaces.AWS;

namespace Application.ApplicantCvs.Commands
{
    public class StartApplicantCvTextDetectionCommand : IRequest
    {
        public byte[] Bytes { get; set; }

        public StartApplicantCvTextDetectionCommand(byte[] bytes)
        {
            Bytes = bytes;
        }
    }

    public class StartApplicantCvTextDetectionCommandHandler : IRequestHandler<StartApplicantCvTextDetectionCommand>
    {
        private readonly ITextParser _parser;
        private readonly IWriteRepository<CvParsingJob> _repository;
        private readonly ICurrentUserContext _currentUserContext;

        public StartApplicantCvTextDetectionCommandHandler(
            ITextParser parser,
            IWriteRepository<CvParsingJob> repository,
            ICurrentUserContext currentUserContext
        )
        {
            _parser = parser;
            _repository = repository;
            _currentUserContext = currentUserContext;
        }

        public async Task<Unit> Handle(StartApplicantCvTextDetectionCommand command, CancellationToken _)
        {
            var (awsId, filePath) = await _parser.StartParsingAsync(command.Bytes);
            UserDto user = await _currentUserContext.GetCurrentUser();

            CvParsingJob job = new CvParsingJob
            {
                TriggerId = user.Id,
                AWSJobId = awsId,
                FilePath = filePath,
            };

            await _repository.CreateAsync(job);

            return Unit.Value;
        }
    }
}
