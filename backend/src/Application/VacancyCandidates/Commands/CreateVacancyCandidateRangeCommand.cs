using Application.Interfaces;
using Application.VacancyCandidates.Dtos;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using Domain.Interfaces.Write;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StageChangeEventType = Domain.Enums.StageChangeEventType;

namespace Application.VacancyCandidates.Commands
{
    public class CreateVacancyCandidateRangeCommand : IRequest<IEnumerable<VacancyCandidateDto>>
    {
        public string[] ApplicantIds { get; set; }
        public string VacancyId { get; set; }

        public CreateVacancyCandidateRangeCommand(string[] applicantIds, string vacancyId)
        {
            ApplicantIds = applicantIds;
            VacancyId = vacancyId;
        }
    }

    public class CreateVacancyCandidateCommandHandler : IRequestHandler<CreateVacancyCandidateRangeCommand, IEnumerable<VacancyCandidateDto>>
    {
        private readonly IWriteRepository<CandidateToStage> _candidateToStageWriteRepository;
        private readonly IStageReadRepository _stageReadRepository;
        private readonly IVacancyCandidateWriteRepository _writeRepository;
        protected readonly ICurrentUserContext _currentUserContext;
        private readonly IMapper _mapper;

        public CreateVacancyCandidateCommandHandler(
            IWriteRepository<CandidateToStage> candidateToStageWriteRepository,
            IStageReadRepository stageReadRepository,
            IVacancyCandidateWriteRepository writeRepository,
            ICurrentUserContext currentUserContext,
            IMapper mapper
        )
        {
            _candidateToStageWriteRepository = candidateToStageWriteRepository;
            _stageReadRepository = stageReadRepository;
            _writeRepository = writeRepository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VacancyCandidateDto>> Handle(CreateVacancyCandidateRangeCommand command, CancellationToken _)
        {
            var user = await _currentUserContext.GetCurrentUser();

            List<VacancyCandidate> candidates = new List<VacancyCandidate>();

            var stageId = (await _stageReadRepository.GetByVacancyIdWithFirstIndex(command.VacancyId)).Id;

            foreach (var id in command.ApplicantIds)
            {
                var vacancyCandidate = new VacancyCandidate
                {
                    ApplicantId = id,
                    DateAdded = DateTime.UtcNow,
                    HrWhoAddedId = user.Id,
                    DomainEvents = new List<DomainEvent>()
                };

                vacancyCandidate.DomainEvents.Add(new CandidateStageChangedEvent(id, command.VacancyId, stageId, StageChangeEventType.Join));

                candidates.Add(vacancyCandidate);
            }


            var result = _mapper.Map<IEnumerable<VacancyCandidateDto>>(await _writeRepository.CreateRangeAsync(candidates.ToArray()));

            foreach (var candidate in result)
            {
                await _candidateToStageWriteRepository.CreateAsync(new CandidateToStage
                {
                    CandidateId = candidate.Id,
                    StageId = stageId,
                    DateAdded = DateTime.UtcNow
                });
            }

            return result;
        }
    }
}
