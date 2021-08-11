using System.Threading;
using System.Threading.Tasks;
using MediatR;
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

        public StartApplicantCvTextDetectionCommandHandler(ITextParser parser)
        {
            _parser = parser;
        }

        public async Task<Unit> Handle(StartApplicantCvTextDetectionCommand command, CancellationToken _)
        {
            await _parser.StartParsingAsync(command.Bytes);
            return Unit.Value;
        }
    }
}
