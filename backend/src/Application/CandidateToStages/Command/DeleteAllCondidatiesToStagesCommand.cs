using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CandidateToStages.Command
{
    public class DeleteAllCondidatiesToStagesCommand : IRequest<Unit>
    {
        public string StageId { get; set; }

        public DeleteAllCondidatiesToStagesCommand(string stageId)
        {
            StageId = stageId;
        }
    }

    public class DeleteCandidateToStagesCommandHandler : IRequestHandler<DeleteAllCondidatiesToStagesCommand, Unit>
    {
        private readonly IWriteRepository<CandidateToStage> _writeCandidateToStageepository;
        private readonly IReadRepository<CandidateToStage> _readCandidateToStageepository;


        public DeleteCandidateToStagesCommandHandler(
            IWriteRepository<CandidateToStage> writeCandidateToStageRepository,
            IReadRepository<CandidateToStage> readCandidateToStageepository
        )
        {
            _writeCandidateToStageepository = writeCandidateToStageRepository;
            _readCandidateToStageepository = readCandidateToStageepository;
        }

        public async Task<Unit> Handle(DeleteAllCondidatiesToStagesCommand command, CancellationToken _)
        {
            var canditaties = (await _readCandidateToStageepository.GetEnumerableAsync())
                .Where(x => x.StageId == command.StageId);
            foreach (var candidate in canditaties)
            {
                await _writeCandidateToStageepository.DeleteAsync(candidate.Id);
            }

            return Unit.Value;
        }
    }
}
