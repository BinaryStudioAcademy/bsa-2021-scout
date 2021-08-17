using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Mail;
using Application.Applicants.Dtos;
using Application.Interfaces;
using Application.Interfaces.AWS;

namespace Application.ApplicantCvs.Commands
{
    public class ParseCvFileToApplicantCommand : IRequest
    {
        public string AWSJobId { get; set; }

        public ParseCvFileToApplicantCommand(string awsJobId)
        {
            AWSJobId = awsJobId;
        }
    }

    public class ParseCvFileToApplicantCommandHandler : IRequestHandler<ParseCvFileToApplicantCommand>
    {
        private readonly string _frontendUrl;
        private readonly ICvParser _parser;
        private readonly ITextParser _textParser;
        private readonly ISmtpFactory _smtp;
        private readonly IReadRepository<CvParsingJob> _repository;
        private readonly IReadRepository<User> _userRepository;

        public ParseCvFileToApplicantCommandHandler(
            ICvParser parser,
            ITextParser textParser,
            ISmtpFactory smtp,
            IReadRepository<CvParsingJob> repository,
            IReadRepository<User> userRepository
        )
        {
            _frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
            _parser = parser;
            _textParser = textParser;
            _smtp = smtp;
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ParseCvFileToApplicantCommand command, CancellationToken _)
        {
            CvParsingJob job = await _repository.GetByPropertyAsync("AWSJobId", command.AWSJobId);
            User user = await _userRepository.GetAsync(job.TriggerId);

            string text = await _textParser.GetText(job.AWSJobId);
            ApplicantCreationVariantsDto dto = await _parser.ParseAsync(text);

            string stringDto = JsonConvert.SerializeObject(dto);
            byte[] bytesDto = Encoding.UTF8.GetBytes(stringDto);
            string base64Dto = Convert.ToBase64String(bytesDto);
            string link = $"{_frontendUrl}/create-variants-applicant?data={base64Dto}";
            string body = string.Format(MailBodyFactory.CV_PARSED, link);

            using (ISmtp connection = _smtp.Connect())
            {
                await connection.SendAsync(
                    user.Email,
                    MailSubjectFactory.CV_PARSED,
                    body
                );
            }

            return Unit.Value;
        }
    }
}
