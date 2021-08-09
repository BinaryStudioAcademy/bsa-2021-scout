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
        public VacancyCandidateReadRepository(IConnectionFactory connectionFactory)
            : base("VacancyCandidates", connectionFactory) { }

        public async Task<VacancyCandidate> GetFullAsync(string id)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT *");
            sql.Append(" FROM VacancyCandidates");
            sql.Append(" LEFT JOIN Applicants ON VacancyCandidates.ApplicantId = Applicants.Id");
            sql.Append(" LEFT JOIN CandidateToStages ON CandidateToStages.CandidateId = VacancyCandidates.Id");
            sql.Append(" LEFT JOIN Stages ON CandidateToStages.StageId = Stages.Id");
            sql.Append(" LEFT JOIN Users ON VacancyCandidates.HrWhoAddedId = Users.Id");
            sql.Append(" LEFT JOIN CandidateReviews ON CandidateReviews.CandidateId = VacancyCandidates.Id");
            sql.Append(" LEFT JOIN Reviews ON CandidateReviews.ReviewId = Reviews.Id");
            sql.Append($" WHERE VacancyCandidates.Id = '{id}'");

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
                    splitOn: "Id,Id,Id,Id,Id,Id"
                );

            VacancyCandidate candidate = resultAsArray.Distinct().FirstOrDefault();

            if (candidate == null || candidate.CandidateToStages.Where(cts => cts.DateRemoved == null).Count() == 0)
            {
                throw new NotFoundException(typeof(VacancyCandidate), id);
            }

            candidate.CandidateToStages = candidate.CandidateToStages
                .OrderBy(cts => cts.DateRemoved)
                .OrderByDescending(cts => cts.DateRemoved.HasValue)
                .ToList();

            return candidate;
        }
    }
}
