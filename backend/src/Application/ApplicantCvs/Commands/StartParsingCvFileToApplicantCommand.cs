using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Interfaces;
using Application.Interfaces.AWS;

namespace Application.ApplicantCvs.Commands
{
    public class StartParsingCvFileToApplicantCommand : IRequest
    {
        public string JobId { get; set; }

        public StartParsingCvFileToApplicantCommand(string jobId)
        {
            JobId = jobId;
        }
    }

    public class StartParsingCvFileToApplicantCommandHandler : IRequestHandler<StartParsingCvFileToApplicantCommand>
    {
        private readonly IReadRepository<CvParsingJob> _repository;
        private readonly IWriteRepository<SkillsParsingJob> _skillsRepository;
        private readonly ICvParser _parser;
        private readonly ITextParser _textParser;

        public StartParsingCvFileToApplicantCommandHandler(
            IReadRepository<CvParsingJob> repository,
            IWriteRepository<SkillsParsingJob> skillsRepository,
            ICvParser parser,
            ITextParser textParser
        )
        {
            _repository = repository;
            _skillsRepository = skillsRepository;
            _parser = parser;
            _textParser = textParser;
        }

        public async Task<Unit> Handle(StartParsingCvFileToApplicantCommand command, CancellationToken _)
        {
            CvParsingJob job = await _repository.GetByPropertyAsync("AWSJobId", command.JobId);
            string text = await _textParser.GetText(job.AWSJobId);

            var (inputPath, outputPath) = await _parser.StartParsingSkillsAsync(text);

            SkillsParsingJob skillsJob = new SkillsParsingJob
            {
                TriggerId = job.TriggerId,
                TextPath = inputPath,
                OutputPath = outputPath,
                OriginalFilePath = job.FilePath,
            };

            await _skillsRepository.CreateAsync(skillsJob);

            return Unit.Value;
        }
    }
}
