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
            sql.Append(" LEFT JOIN Stages ON EXISTS");
            sql.Append("(SELECT Id");
            sql.Append(" FROM CandidateToStages");
            sql.Append(" WHERE CandidateToStages.CandidateId = VacancyCandidates.Id");
            sql.Append(" AND CandidateToStages.StageId = Stages.Id AND CandidateToStages.DateRemoved IS NULL)");
            sql.Append(" LEFT JOIN Users ON VacancyCandidates.HrWhoAddedId = Users.Id");
            sql.Append(" LEFT JOIN CandidateReviews ON CandidateReviews.CandidateId = VacancyCandidates.Id");
            sql.Append(" LEFT JOIN Stages ON CandidateReviews.StageId = Stages.Id");
            sql.Append(" LEFT JOIN Reviews ON CandidateReviews.ReviewId = Reviews.Id");
            sql.Append($" WHERE VacancyCandidates.Id = {id}");

            Dictionary<string, CandidateReview> candidateReviewDictionary = new Dictionary<string, CandidateReview>();
            VacancyCandidate cachedCandidate = null;

            IEnumerable<VacancyCandidate> resultAsArray = await connection
                .QueryAsync<VacancyCandidate, Applicant, Stage, User, CandidateReview, Stage, Review, VacancyCandidate>(
                    sql.ToString(),
                    (candidate, applicant, stage, hrWhoAdded, candidateReview, candidateReviewStage, review) =>
                    {
                        if (cachedCandidate == null)
                        {
                            cachedCandidate = candidate;
                        }

                        cachedCandidate.Applicant = applicant;
                        cachedCandidate.HrWhoAdded = hrWhoAdded;

                        if (candidateReview != null)
                        {
                            CandidateReview candidateReviewEntry;

                            if (!candidateReviewDictionary.TryGetValue(candidateReview.Id, out candidateReviewEntry))
                            {
                                candidateReviewEntry = candidateReview;
                                candidateReviewDictionary.Add(candidateReviewEntry.Id, candidateReviewEntry);
                                cachedCandidate.Reviews.Add(candidateReviewEntry);
                            }

                            candidateReviewEntry.Stage = candidateReviewStage;
                            candidateReviewEntry.Review = review;
                        }

                        if (stage != null)
                        {
                            cachedCandidate.CandidateToStages = new List<CandidateToStage> {
                                new CandidateToStage { Stage = stage },
                            };
                        }

                        return cachedCandidate;
                    },
                    splitOn: "Id,Id,Id,Id,Id,Id"
                );

            VacancyCandidate candidate = resultAsArray.Distinct().FirstOrDefault();

            if (candidate == null)
            {
                throw new NotFoundException(typeof(VacancyCandidate), id);
            }

            return candidate;
        }
    }
}
