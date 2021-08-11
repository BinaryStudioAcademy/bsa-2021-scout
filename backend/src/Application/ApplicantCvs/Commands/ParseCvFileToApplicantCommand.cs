using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Application.Applicants.Dtos;
using Application.Interfaces;
using Application.Interfaces.AWS;

namespace Application.ApplicantCvs.Commands
{
    public class ParseCvFileToApplicantCommand : IRequest<ApplicantCreationVariantsDto>
    {
        public string Text { get; set; }

        public ParseCvFileToApplicantCommand(string text)
        {
            Text = text;
        }
    }

    public class ParseCvFileToApplicantCommandHandler
        : IRequestHandler<ParseCvFileToApplicantCommand, ApplicantCreationVariantsDto>
    {
        private readonly ICvParser _parser;

        public ParseCvFileToApplicantCommandHandler(ICvParser parser)
        {
            _parser = parser;
        }

        public async Task<ApplicantCreationVariantsDto> Handle(ParseCvFileToApplicantCommand command, CancellationToken _)
        {
            ApplicantCreationVariantsDto dto = await _parser.ParseAsync(command.Text);
            return dto;
        }
    }
}
