using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Archive.Commands
{
    public class DeleteArhivedVacancyCommand : IRequest<Unit>
    {
        public string VacancyId { get; set; }
        public DeleteArhivedVacancyCommand(string vacancyId)
        {
            VacancyId = vacancyId;
        }
    }

    public class DeleteArhivedVacancyCommandHandler : IRequestHandler<DeleteArhivedVacancyCommand, Unit>
    {
        private readonly IWriteRepository<Vacancy> _writeRepository;
        private readonly IWriteRepository<ArchivedEntity> _archivedEntityWriteRepository;
        private readonly IArchivedEntityReadRepository _archivedEntityReadRepository;
        private readonly IWriteRepository<UserFollowedEntity> _userFollowedEntityWriteRepository;
        private readonly IUserFollowedReadRepository _userFollowedEntityReadRepository;
        private readonly IWriteRepository<Stage> _writeStageRepository;
        private readonly IReadRepository<Stage> _readStageRepository;
        private readonly IReadRepository<CandidateToStage> _readCandidateToStageRepository;
        private readonly IWriteRepository<VacancyCandidate> _writeVacancyCandidateRepository;
        private readonly IReadRepository<VacancyCandidate> _readVacancyCandidateRepository;
        private readonly IReadRepository<CandidateReview> _readCandidateReviewRepository;
        private readonly IReadRepository<ReviewToStage> _readReviewToStageRepository;
        private readonly IWriteRepository<Review> _writeReviewRepository;
        private readonly IReadRepository<Review> _readReviewRepository;
        private readonly IElasticWriteRepository<ElasticEntity> _vacancyElasticWriteRepository;

        public DeleteArhivedVacancyCommandHandler(
            IWriteRepository<Vacancy> writeRepository,
            IWriteRepository<ArchivedEntity> archivedEntityWriteRepository,
            IArchivedEntityReadRepository archivedEntityReadRepository,
            IWriteRepository<UserFollowedEntity> userFollowedEntityWriteRepository,
            IUserFollowedReadRepository userFollowedEntityReadRepository,
            IWriteRepository<Stage> writeStageRepository,
            IReadRepository<Stage> readStageRepository,
            IReadRepository<CandidateToStage> readCandidateToStageRepository,
            IWriteRepository<VacancyCandidate> writeVacancyCandidateRepository,
            IReadRepository<VacancyCandidate> readVacancyCandidateRepository,
            IReadRepository<CandidateReview> readCandidateReviewRepository,
            IReadRepository<ReviewToStage> readReviewToStageRepository,
            IWriteRepository<Review> writeReviewRepository,
            IReadRepository<Review> readReviewRepository,
            IElasticWriteRepository<ElasticEntity> vacancyElasticWriteRepository)
        {
            _writeRepository = writeRepository;
            _archivedEntityWriteRepository = archivedEntityWriteRepository;
            _archivedEntityReadRepository = archivedEntityReadRepository;
            _userFollowedEntityWriteRepository = userFollowedEntityWriteRepository;
            _userFollowedEntityReadRepository = userFollowedEntityReadRepository;
            _writeStageRepository = writeStageRepository;
            _readStageRepository = readStageRepository;
            _readCandidateToStageRepository = readCandidateToStageRepository;
            _writeVacancyCandidateRepository = writeVacancyCandidateRepository;
            _readVacancyCandidateRepository = readVacancyCandidateRepository;
            _readCandidateReviewRepository = readCandidateReviewRepository;
            _readReviewToStageRepository = readReviewToStageRepository;
            _writeReviewRepository = writeReviewRepository;
            _readReviewRepository = readReviewRepository;
            _vacancyElasticWriteRepository = vacancyElasticWriteRepository;
        }

        public async Task<Unit> Handle(DeleteArhivedVacancyCommand command, CancellationToken _)
        {
            var archivedVacancy = await _archivedEntityReadRepository.GetByEntityTypeAndIdAsync(EntityType.Vacancy, command.VacancyId);
            if (archivedVacancy is not null)
            {
                await _archivedEntityWriteRepository.DeleteAsync(archivedVacancy.Id);
            }
           
            var followedVacancy = await _userFollowedEntityReadRepository.GetForCurrentUserByTypeAndEntityId(command.VacancyId, EntityType.Vacancy);
            if (followedVacancy is not null)
            {
                await _userFollowedEntityWriteRepository.DeleteAsync(followedVacancy.Id);
            }

            IEnumerable<string> stageIds = (await _readStageRepository.GetEnumerableByPropertyAsync(nameof(Stage.VacancyId), command.VacancyId))
                                           .Select(stage => stage.Id);

            foreach (var stageId in stageIds)
            {
                await DeleteDependenciesAsync(stageId);
                await _writeStageRepository.DeleteAsync(stageId);
            }

            await _writeRepository.DeleteAsync(command.VacancyId);
            await _vacancyElasticWriteRepository.DeleteAsync(command.VacancyId);

            return Unit.Value;
        }

        private async Task DeleteDependenciesAsync(string stageId)
        {
            var reviewsIds = new List<Review>();

            IEnumerable<CandidateReview> candidateReviewsIds = await _readCandidateReviewRepository.GetEnumerableByPropertyAsync(nameof(CandidateReview.StageId), stageId);
            foreach (var candidateReviewsId in candidateReviewsIds)
            {
                reviewsIds.AddRange(await _readReviewRepository.GetEnumerableByPropertyAsync(nameof(Review.Id), candidateReviewsId.ReviewId));         
            }

            IEnumerable<ReviewToStage> stageReviewsIds = await _readReviewToStageRepository.GetEnumerableByPropertyAsync(nameof(ReviewToStage.StageId), stageId);
            foreach (var stageReviewsId in stageReviewsIds)
            {
                reviewsIds.AddRange(await _readReviewRepository.GetEnumerableByPropertyAsync(nameof(Review.Id), stageReviewsId.ReviewId));
            }

            foreach (var reviewId in reviewsIds.Select(p => p.Id).Distinct())
            {
                await _writeReviewRepository.DeleteAsync(reviewId);
            }       

            IEnumerable<CandidateToStage> candidateToStagesIds = await _readCandidateToStageRepository.GetEnumerableByPropertyAsync(nameof(CandidateToStage.StageId), stageId);
            var vacancyCandidatesIds = new List<VacancyCandidate>();
            reviewsIds = new List<Review>();

            foreach (var candidateToStageId in candidateToStagesIds)
            {
                vacancyCandidatesIds.AddRange(await _readVacancyCandidateRepository.GetEnumerableByPropertyAsync(nameof(VacancyCandidate.Id), candidateToStageId.CandidateId));
            }

            foreach (var candidateId in vacancyCandidatesIds.Select(p => p.Id).Distinct())
            {
                candidateReviewsIds = await _readCandidateReviewRepository.GetEnumerableByPropertyAsync(nameof(CandidateReview.CandidateId), candidateId);
                foreach (var candidateReviewsId in candidateReviewsIds)
                {
                    reviewsIds.AddRange(await _readReviewRepository.GetEnumerableByPropertyAsync(nameof(Review.Id), candidateReviewsId.ReviewId));
                }

                foreach (var reviewId in reviewsIds.Select(p => p.Id).Distinct())
                {
                    await _writeReviewRepository.DeleteAsync(reviewId);
                }

                await _writeVacancyCandidateRepository.DeleteAsync(candidateId);
            }
        }
    }
}