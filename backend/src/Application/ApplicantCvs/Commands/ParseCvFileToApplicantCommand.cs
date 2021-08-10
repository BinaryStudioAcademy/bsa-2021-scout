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
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }

        public ParseCvFileToApplicantCommand(byte[] bytes, string contentType)
        {
            Bytes = bytes;
            ContentType = contentType;
        }
    }

    public class ParseCvFileToApplicantCommandHandler
        : IRequestHandler<ParseCvFileToApplicantCommand, ApplicantCreationVariantsDto>
    {
        private readonly ICvParser _parser;
        private readonly ITextParser _textParser;

        public ParseCvFileToApplicantCommandHandler(ICvParser parser, ITextParser textParser)
        {
            _parser = parser;
            _textParser = textParser;
        }

        public async Task<ApplicantCreationVariantsDto> Handle(ParseCvFileToApplicantCommand command, CancellationToken _)
        {
            string text;

            if (command.ContentType == "text/plain")
            {
                text = Encoding.UTF8.GetString(command.Bytes);
            }
            else
            {
                text = await _textParser.ParseAsync(command.Bytes);
            }

            ApplicantCreationVariantsDto dto = await _parser.ParseAsync(text);

            return dto;
        }
    }
}
