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

        public DeleteVacancyStageCommandHandler(
            IWriteRepository<Stage> writeStageRepository,
            ISender mediator
        )
        {
            _writeStageRepository = writeStageRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteVacancyStageCommand command, CancellationToken _)
        {
            await _mediator.Send(new DeleteAllCondidatiesToStagesCommand(command.StageId));
            await _writeStageRepository.DeleteAsync(command.StageId);

            return Unit.Value;
        }
    }
}