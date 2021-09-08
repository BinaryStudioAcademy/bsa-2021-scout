using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using Newtonsoft.Json;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Mail;
using Application.Interfaces;
using Application.Interfaces.AWS;
using Application.Comprehend.Dtos;
using Application.Applicants.Dtos;

namespace Application.ApplicantCvs.Commands
{
    public class FinishParsingCvFileToApplicantCommand : IRequest
    {
        public string OutputPath { get; set; }

        public FinishParsingCvFileToApplicantCommand(string outputPath)
        {
            OutputPath = outputPath;
        }
    }

    public class FinishParsingCvFileToApplicantCommandHandler : IRequestHandler<FinishParsingCvFileToApplicantCommand>
    {
        private readonly IReadRepository<SkillsParsingJob> _repository;
        private readonly IReadRepository<User> _userRepository;
        private readonly ICvParser _parser;
        private readonly IS3Uploader _s3;
        private readonly IAwsS3ReadRepository _s3ReadRepository;
        private readonly IComprehendParser _comprehend;
        private readonly ISmtpFactory _smtp;
        private readonly string _frontendUrl;

        public FinishParsingCvFileToApplicantCommandHandler(
            IReadRepository<SkillsParsingJob> repository,
            IReadRepository<User> userRepository,
            ICvParser parser,
            IS3Uploader s3,
            IAwsS3ReadRepository s3ReadRepository,
            IComprehendParser comprehend,
            ISmtpFactory smtp
        )
        {
            _frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
            _repository = repository;
            _userRepository = userRepository;
            _parser = parser;
            _s3 = s3;
            _s3ReadRepository = s3ReadRepository;
            _comprehend = comprehend;
            _smtp = smtp;
        }

        public async Task<Unit> Handle(FinishParsingCvFileToApplicantCommand command, CancellationToken _)
        {
            string correctOutPath = string.Join("/", command.OutputPath.Split("/").Take(2));
            SkillsParsingJob job = await _repository.GetByPropertyAsync("OutputPath", correctOutPath);
            User user = await _userRepository.GetAsync(job.TriggerId);

            byte[] textBytes = await _s3.ReadAsync(job.TextPath);
            string text = Encoding.UTF8.GetString(textBytes);
            byte[] outputTarGZipBytes = await _s3.ReadAsync(command.OutputPath);
            string output = await _comprehend.TarGZipOutputToString(outputTarGZipBytes);

            ParsedEntitiesDto parsed = JsonConvert.DeserializeObject<ParsedEntitiesDto>(output);
            ApplicantCreationVariantsDto dto = await _parser.FinishParsingAsync(text, parsed.Entities);

            IEnumerable<string> pathItems = job.OriginalFilePath.Split("/");
            dto.Cv = await _s3ReadRepository.GetPublicUrlAsync(pathItems.First(), pathItems.Last());

            string stringDto = JsonConvert.SerializeObject(dto);
            byte[] bytesDto = Encoding.UTF8.GetBytes(stringDto);
            string base64 = Convert.ToBase64String(bytesDto);
            string url = $"{_frontendUrl}/applicants?variants=1&data={base64}";
            string body = string.Format(MailBodyFactory.CV_PARSED, url);

            using (ISmtp connection = await _smtp.Connect())
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
