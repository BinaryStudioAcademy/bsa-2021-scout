using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.CandidateToStages.Command;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;

namespace Application.Stages.Commands
{
    public class DeleteVacancyStageCommand : IRequest<Unit>
    {
        public string StageId { get; set; }

        public DeleteVacancyStageCommand(string stageId)
        {
            StageId = stageId;
        }
    }

    public class DeleteVacancyStageCommandHandler : IRequestHandler<DeleteVacancyStageCommand, Unit>
    {
        private readonly IWriteRepository<Stage> _writeStageRepository;
        private readonly ISender _mediator;
        private readonly IWriteRepository<Action> _writeActionRepository;
        private readonly IReadRepository<Action> _readActionRepository;

        public DeleteVacancyStageCommandHandler(
            IWriteRepository<Stage> writeStageRepository,
            IWriteRepository<Action> writeActionRepository,
            IReadRepository<Action> readActionRepository,
            ISender mediator
        )
        {
            _writeStageRepository = writeStageRepository;
            _mediator = mediator;
            _writeActionRepository = writeActionRepository;
            _readActionRepository = readActionRepository;
        }

        public async Task<Unit> Handle(DeleteVacancyStageCommand command, CancellationToken _)
        {
            var actions = ((List<Action>)(await _readActionRepository.GetEnumerableAsync()))
                .FindAll(x => x.StageId == command.StageId);
            foreach (var action in actions)
            {
               await _writeActionRepository.DeleteAsync(action.Id);
            }
            await _mediator.Send(new DeleteAllCondidatiesToStagesCommand(command.StageId));
            await _writeStageRepository.DeleteAsync(command.StageId);
            
            

            return Unit.Value;
        }
    }
}