using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Interfaces.AWS;

namespace Application.ApplicantCvs.Commands
{
    public class StartApplicantCvTextDetectionCommand : IRequest
    {
        public byte[] Bytes { get; set; }
        public string UserId { get; set; }

        public StartApplicantCvTextDetectionCommand(byte[] bytes, string userId)
        {
            Bytes = bytes;
            UserId = userId;
        }
    }

    public class StartApplicantCvTextDetectionCommandHandler : IRequestHandler<StartApplicantCvTextDetectionCommand>
    {
        private readonly ITextParser _parser;
        private readonly IWriteRepository<CvParsingJob> _repository;

        public StartApplicantCvTextDetectionCommandHandler(ITextParser parser, IWriteRepository<CvParsingJob> repository)
        {
            _parser = parser;
            _repository = repository;
        }

        public async Task<Unit> Handle(StartApplicantCvTextDetectionCommand command, CancellationToken _)
        {
            string awsId = await _parser.StartParsingAsync(command.Bytes);

            CvParsingJob job = new CvParsingJob
            {
                TriggerId = command.UserId,
                AWSJobId = awsId,
            };

            await _repository.CreateAsync(job);

            return Unit.Value;
        }
    }
}
