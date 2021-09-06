using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Events;
using Domain.Entities;
using Domain.Interfaces.Write;
using Domain.Interfaces.Abstractions;
using Application.Common.Exceptions;
using Application.VacancyCandidates.Dtos;
using Domain.Interfaces.Read;
using System;
using StageChangeEventType = Domain.Enums.StageChangeEventType;

namespace Application.VacancyCandidates.Commands
{
    public class ChangeCandidateStageCommand : IRequest<VacancyCandidateDto>
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public string StageId { get; set; }
        public string VacancyId { get; set; }

        public ChangeCandidateStageCommand(string userId, string id, string vacancyId, string stageId)
        {
            UserId = userId;
            Id = id;
            StageId = stageId;
            VacancyId = vacancyId;
        }
    }

    public class ChangeCandidateStageCommandHandler : IRequestHandler<ChangeCandidateStageCommand, VacancyCandidateDto>
    {
        private readonly IReadRepository<VacancyCandidate> _readRepository;
        private readonly IWriteRepository<VacancyCandidate> _writeRepository;
        private readonly ICandidateToStageReadRepository _candidateToStageReadRepository;
        private readonly IWriteRepository<CandidateToStage> _candidateToStageWriteRepository;
        private readonly IMapper _mapper;

        public ChangeCandidateStageCommandHandler(
            IReadRepository<VacancyCandidate> readRepository,
            IWriteRepository<VacancyCandidate> writeRepository,
            ICandidateToStageReadRepository candidateToStageReadRepository,
            IWriteRepository<CandidateToStage> candidateToStageWriteRepository,
            IMapper mapper
        )
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _candidateToStageReadRepository = candidateToStageReadRepository;
            _candidateToStageWriteRepository = candidateToStageWriteRepository;
            _mapper = mapper;
        }

        public async Task<VacancyCandidateDto> Handle(ChangeCandidateStageCommand command, CancellationToken _)
        {
            VacancyCandidate candidate = await _readRepository.GetAsync(command.Id);

            if (candidate == null)
            {
                throw new NotFoundException(typeof(VacancyCandidate), command.Id);
            }

            CandidateToStage oldStage = await _candidateToStageReadRepository
                .GetCurrentForCandidateByVacancyAsync(command.Id, command.VacancyId);

            oldStage.DateRemoved = DateTime.UtcNow;
            await _candidateToStageWriteRepository.UpdateAsync(oldStage);

            CandidateToStage newStage = new CandidateToStage
            {
                CandidateId = command.Id,
                StageId = command.StageId,
                MoverId = command.UserId,
                DateAdded = DateTime.UtcNow,
            };

            await _candidateToStageWriteRepository.CreateAsync(newStage);

            CandidateStageChangedEvent stageLeaveEvent = new CandidateStageChangedEvent(
                command.Id,
                command.VacancyId,
                oldStage.StageId,
                StageChangeEventType.Leave
            );

            CandidateStageChangedEvent stageJoinEvent = new CandidateStageChangedEvent(
                command.Id,
                command.VacancyId,
                command.StageId,
                StageChangeEventType.Join
            );

            candidate.DomainEvents.Add(stageLeaveEvent);
            candidate.DomainEvents.Add(stageJoinEvent);

            await _writeRepository.UpdateAsync(candidate);

            return _mapper.Map<VacancyCandidate, VacancyCandidateDto>(candidate);
        }
    }
}
