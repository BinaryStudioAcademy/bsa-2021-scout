using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Application.Common.Exceptions;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class VacancyCandidateReadRepository : ReadRepository<VacancyCandidate>, IVacancyCandidateReadRepository
    {
        private readonly IApplicantReadRepository _applicantRepository;

        public VacancyCandidateReadRepository(
            IConnectionFactory connectionFactory,
            IApplicantReadRepository applicantRepository
        ) : base("VacancyCandidates", connectionFactory)
        {
            _applicantRepository = applicantRepository;
        }

        public async Task<VacancyCandidate> GetFullAsync(string id, string vacancyId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT *");
            sql.Append(" FROM VacancyCandidates");
            sql.Append(" LEFT JOIN Applicants ON VacancyCandidates.ApplicantId = Applicants.Id");
            sql.Append(" LEFT JOIN CandidateToStages ON CandidateToStages.CandidateId = VacancyCandidates.Id");
            sql.Append(" AND EXISTS");
            sql.Append("(SELECT Id");
            sql.Append(" FROM Stages");
            sql.Append(" WHERE Stages.Id = CandidateToStages.StageId");
            sql.Append(" AND Stages.VacancyId = @vacancyId)");
            sql.Append(" LEFT JOIN Stages ON CandidateToStages.StageId = Stages.Id");
            sql.Append(" LEFT JOIN Users ON VacancyCandidates.HrWhoAddedId = Users.Id");
            sql.Append(" LEFT JOIN CandidateReviews ON CandidateReviews.CandidateId = VacancyCandidates.Id");
            sql.Append(" LEFT JOIN Reviews ON CandidateReviews.ReviewId = Reviews.Id");
            sql.Append(" WHERE VacancyCandidates.Id = @id");

            Dictionary<string, CandidateReview> candidateReviewDict = new Dictionary<string, CandidateReview>();
            Dictionary<string, CandidateToStage> candidateToStageDict = new Dictionary<string, CandidateToStage>();
            VacancyCandidate cachedCandidate = null;

            IEnumerable<VacancyCandidate> resultAsArray = await connection
                .QueryAsync<VacancyCandidate, Applicant, CandidateToStage, Stage, User, CandidateReview, Review, VacancyCandidate>(
                    sql.ToString(),
                    (candidate, applicant, candidateToStage, stage, hrWhoAdded, candidateReview, review) =>
                    {
                        if (cachedCandidate == null)
                        {
                            cachedCandidate = candidate;
                            cachedCandidate.Reviews = new List<CandidateReview>();
                            cachedCandidate.CandidateToStages = new List<CandidateToStage>();
                        }

                        cachedCandidate.Applicant = applicant;
                        cachedCandidate.HrWhoAdded = hrWhoAdded;

                        if (candidateReview != null)
                        {
                            CandidateReview candidateReviewEntry;

                            if (!candidateReviewDict.TryGetValue(candidateReview.Id, out candidateReviewEntry))
                            {
                                candidateReviewEntry = candidateReview;
                                candidateReviewDict.Add(candidateReviewEntry.Id, candidateReviewEntry);
                                cachedCandidate.Reviews.Add(candidateReviewEntry);
                            }

                            candidateReviewEntry.Review = review;
                        }

                        if (candidateToStage != null)
                        {
                            CandidateToStage candidateToStageEntry;

                            if (!candidateToStageDict.TryGetValue(candidateToStage.Id, out candidateToStageEntry))
                            {
                                candidateToStageEntry = candidateToStage;
                                candidateToStageDict.Add(candidateToStageEntry.Id, candidateToStageEntry);
                                cachedCandidate.CandidateToStages.Add(candidateToStageEntry);
                            }

                            if (stage != null)
                            {
                                candidateToStageEntry.Stage = stage;
                            }
                        }

                        return cachedCandidate;
                    },
                    new
                    {
                        id = id,
                        vacancyId = vacancyId,
                    },
                    splitOn: "Id,Id,Id,Id,Id,Id"
                );

            await connection.CloseAsync();

            VacancyCandidate candidate = resultAsArray.Distinct().FirstOrDefault();

            // Oops! Ran out of JOINs in the first query!

            try
            {
                FileInfo cvInfo = await _applicantRepository.GetCvFileInfoAsync(candidate.ApplicantId);
                candidate.Applicant.CvFileInfo = cvInfo;
            }
            catch
            {
                //
            }

            try
            {
                FileInfo photoInfo = await _applicantRepository.GetPhotoFileInfoAsync(candidate.ApplicantId);
                candidate.Applicant.PhotoFileInfo = photoInfo;
            }
            catch
            {
                //
            }

            if (candidate == null || candidate.CandidateToStages.Where(cts => cts.DateRemoved == null).Count() == 0)
            {
                throw new NotFoundException(typeof(VacancyCandidate), id);
            }

            candidate.CandidateToStages = candidate.CandidateToStages
                .OrderByDescending(cts => cts.DateAdded)
                .ToList();

            return candidate;
        }

        public async Task<VacancyCandidate> GetFullByApplicantAndStageAsync(string applicantId, string stageId)
        {
            using var connection = _connectionFactory.GetSqlConnection();

            string sql = @$"SELECT VC.*
                            FROM CandidateToStages AS CtS
                            JOIN VacancyCandidates AS VC ON VC.Id = CtS.CandidateId
                            JOIN Applicants AS A ON A.Id=VC.ApplicantId
                            WHERE CtS.StageId = @stageId
                            AND VC.ApplicantId = @applicantId";

            return await connection.QueryFirstOrDefaultAsync<VacancyCandidate>(sql, new
            {
                applicantId = @applicantId,
                stageId = @stageId
            });
        }
    }
}
