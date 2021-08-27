using Application.VacancyCandidates.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using Domain.Interfaces.Write;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.VacancyCandidates.Commands
{
    public class CreateVacancyCandidateNoAuthCommand : IRequest<VacancyCandidateDto>
    {
        public string Id { get; set; }
        public string VacancyId { get; set; }

        public CreateVacancyCandidateNoAuthCommand(string id, string vacancyId)
        {
            Id = id;
            VacancyId = vacancyId;
        }
    }

    public class CreateVacancyCandidateNoAuthCommandHandler : IRequestHandler<CreateVacancyCandidateNoAuthCommand, VacancyCandidateDto>
    {
        private readonly IWriteRepository<CandidateToStage> _candidateToStageWriteRepository;
        private readonly IReadRepository<CandidateToStage> _candidateToStageReadRepository;
        private readonly IStageReadRepository _stageReadRepository;
        private readonly IVacancyCandidateWriteRepository _writeRepository;
        private readonly IVacancyCandidateReadRepository _readRepository;
        private readonly IMapper _mapper;

        public CreateVacancyCandidateNoAuthCommandHandler(
            IWriteRepository<CandidateToStage> candidateToStageWriteRepository,
            IReadRepository<CandidateToStage> candidateToStageReadRepository,
            IStageReadRepository stageReadRepository,
            IVacancyCandidateWriteRepository writeRepository,
            IVacancyCandidateReadRepository readRepository,
            IMapper mapper
        )
        {
            _candidateToStageWriteRepository = candidateToStageWriteRepository;
            _candidateToStageReadRepository = candidateToStageReadRepository;
            _stageReadRepository = stageReadRepository;
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
        }

        public async Task<VacancyCandidateDto> Handle(CreateVacancyCandidateNoAuthCommand command, CancellationToken _)
        {
            var stageId = (await _stageReadRepository.GetByVacancyIdWithZeroIndex(command.VacancyId)).Id;
            var vacancyCandidate = await _readRepository.GetFullByApplicantAndStageAsync(command.Id, stageId);

            VacancyCandidateDto vacancyCandidateDto = null;

            if (vacancyCandidate == null)
            {
                var candidate = new VacancyCandidate
                {
                    ApplicantId = command.Id,
                    DateAdded = DateTime.Now,
                    IsSelfApplied = true,
                };

                vacancyCandidateDto = _mapper.Map<VacancyCandidateDto>(await _writeRepository.CreateAsync(candidate));

                await _candidateToStageWriteRepository.CreateAsync(new CandidateToStage
                {
                    CandidateId = candidate.Id,
                    StageId = stageId,
                    DateAdded = DateTime.Now
                });
            }

            return vacancyCandidateDto;
        }
    }
}
