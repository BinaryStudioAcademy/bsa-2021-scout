using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Write;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Action = Domain.Entities.Action;

namespace Application.Vacancies.Commands.Delete
{
    public class DeleteVacancyCommand : IRequest
    {
        public string Id { get; set; }
        public DeleteVacancyCommand(string id)
        {
            Id = id;
        }
    }

    public class DeleteVacancyCommandHandler : IRequestHandler<DeleteVacancyCommand>
    {
        private readonly IWriteRepository<Vacancy> _writeRepository;
        private readonly IWriteRepository<Stage> _writeStageRepository;
        private readonly IReadRepository<Stage> _readStageRepository;
        private readonly ICandidateToStageWriteRepository _writeCandidateToStageRepository;
        private readonly IReadRepository<CandidateToStage> _readCandidateToStageRepository;
        private readonly IWriteRepository<Action> _writeActionRepository;
        private readonly IReadRepository<Action> _readActionRepository;
        private readonly IWriteRepository<CandidateReview> _writeCandidateReviewRepository;
        private readonly IReadRepository<CandidateReview> _readCandidateReviewRepository;
        private readonly IWriteRepository<ReviewToStage> _writeReviewToStageRepository;
        private readonly IReadRepository<ReviewToStage> _readReviewToStageRepository;

        public DeleteVacancyCommandHandler(IWriteRepository<Vacancy> writeRepository,
            IWriteRepository<Stage> writeStageRepository,
            IReadRepository<Stage> readStageRepository,
            ICandidateToStageWriteRepository writeCandidateToStageRepository,
            IReadRepository<CandidateToStage> readCandidateToStageRepository,
            IWriteRepository<Action> writeActionRepository,
            IReadRepository<Action> readActionRepository,
            IWriteRepository<CandidateReview> writeCandidateReviewRepository,
            IReadRepository<CandidateReview> readCandidateReviewRepository,
            IWriteRepository<ReviewToStage> writeReviewToStageRepository,
            IReadRepository<ReviewToStage> readReviewToStageRepository)
        {
            _writeRepository = writeRepository;
            _writeStageRepository = writeStageRepository;
            _readStageRepository = readStageRepository;
            _writeCandidateToStageRepository = writeCandidateToStageRepository;
            _readCandidateToStageRepository = readCandidateToStageRepository;
            _writeActionRepository = writeActionRepository;
            _readActionRepository = readActionRepository;
            _writeCandidateReviewRepository = writeCandidateReviewRepository;
            _readCandidateReviewRepository = readCandidateReviewRepository;
            _writeReviewToStageRepository = writeReviewToStageRepository;
            _readReviewToStageRepository = readReviewToStageRepository;
        }

        public async Task<Unit> Handle(DeleteVacancyCommand command, CancellationToken _)
        {

            var stageIds = (await _readStageRepository.GetEnumerableAsync())
            .Where(stage => stage.VacancyId == command.Id)
            .Select(stage => stage.Id);

            foreach (var stageId in stageIds)
            {
                await DeleteDependenciesAsync(stageId);
                await _writeStageRepository.DeleteAsync(stageId);
            }

            await _writeRepository.DeleteAsync(command.Id);

            return await Task.FromResult<Unit>(Unit.Value);
        }

        private async Task DeleteDependenciesAsync(string stageId)
        {
            var candidateToStagesIds = (await _readCandidateToStageRepository.GetEnumerableAsync())
                .Where(candidateToStage => candidateToStage.StageId == stageId);

            var actionsIds = (await _readActionRepository.GetEnumerableAsync())
                .Where(actions => actions.StageId == stageId);

            var candidateReviewsIds = (await _readCandidateReviewRepository.GetEnumerableAsync())
                .Where(candidateReview => candidateReview.StageId == stageId);

            var stageReviewsIds = (await _readReviewToStageRepository.GetEnumerableAsync())
                .Where(reviewToStage => reviewToStage.StageId == stageId);

            foreach (var candidateToStagesId in candidateToStagesIds)
            {
                await _writeCandidateToStageRepository.DeleteAsync(candidateToStagesId.Id);
            }

            foreach (var actionsId in actionsIds)
            {
                await _writeActionRepository.DeleteAsync(actionsId.Id);
            }

            foreach (var candidateReviewsId in candidateReviewsIds)
            {
                await _writeCandidateReviewRepository.DeleteAsync(candidateReviewsId.Id);
            }

            foreach (var stageReviewsId in stageReviewsIds)
            {
                await _writeReviewToStageRepository.DeleteAsync(stageReviewsId.Id);
            }
        }
    }
}
