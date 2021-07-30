using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Domain.Events;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Exceptions;
using Application.VacancyCandidates.Dtos;

namespace Application.VacancyCandidates.Commands
{
    public class ChangeCandidateStageCommand : IRequest<VacancyCandidateDto>
    {
        public string Id { get; set; }
        public string StageId { get; set; }

        public ChangeCandidateStageCommand(string id, string stageId)
        {
            Id = id;
            StageId = stageId;
        }
    }

    public class ChangeCandidateStageCommandHandler : IRequestHandler<ChangeCandidateStageCommand, VacancyCandidateDto>
    {
        private readonly IReadRepository<VacancyCandidate> _readRepository;
        private readonly IWriteRepository<VacancyCandidate> _writeRepository;
        private readonly IMapper _mapper;

        public ChangeCandidateStageCommandHandler(
            IReadRepository<VacancyCandidate> readRepository,
            IWriteRepository<VacancyCandidate> writeRepository,
            IMapper mapper
        )
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
        }

        public async Task<VacancyCandidateDto> Handle(ChangeCandidateStageCommand command, CancellationToken _)
        {
            VacancyCandidate candidate = await _readRepository.GetAsync(command.Id);

            if (candidate == null)
            {
                throw new NotFoundException(typeof(VacancyCandidate), command.Id);
            }

            var changedEvent = new CandidateStageChangedEvent(command.Id, command.StageId);

            candidate.StageId = command.StageId;
            candidate.DomainEvents.Add(changedEvent);

            await _writeRepository.UpdateAsync(candidate);

            return _mapper.Map<VacancyCandidate, VacancyCandidateDto>(candidate);
        }
    }
}
