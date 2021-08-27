using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Interfaces.Read;
using Domain.Entities;
using Application.Common.Exceptions;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Repositories.Read
{
    public class StageReadRepository : ReadRepository<Stage>, IStageReadRepository
    {
        public StageReadRepository(IConnectionFactory connectionFactory) : base("Stages", connectionFactory) { }

        public async Task<Vacancy> GetByVacancyAsync(string vacancyId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT");
            sql.Append(" Vacancies.Id,");
            sql.Append(" Vacancies.Title,");
            sql.Append(" Stages.*,");
            sql.Append(" Reviews.*,");
            sql.Append(" VacancyCandidates.*,");
            sql.Append(" CandidateReviews.*,");
            sql.Append(" Applicants.*");
            sql.Append(" FROM Vacancies");
            sql.Append(" LEFT JOIN Stages ON Stages.VacancyId = Vacancies.Id");
            sql.Append(" LEFT JOIN Reviews ON EXISTS");
            sql.Append("(SELECT Id");
            sql.Append(" FROM ReviewToStages");
            sql.Append(" WHERE ReviewToStages.StageId = Stages.Id");
            sql.Append(" AND ReviewToStages.ReviewId = Reviews.Id)");
            sql.Append(" LEFT JOIN VacancyCandidates ON EXISTS");
            sql.Append("(SELECT Id");
            sql.Append(" FROM CandidateToStages");
            sql.Append(" WHERE CandidateToStages.CandidateId = VacancyCandidates.Id");
            sql.Append(" AND CandidateToStages.StageId = Stages.Id");
            sql.Append(" AND CandidateToStages.DateRemoved IS NULL)");
            sql.Append(" LEFT JOIN CandidateReviews ON CandidateReviews.CandidateId = VacancyCandidates.Id");
            sql.Append(" LEFT JOIN Applicants ON VacancyCandidates.ApplicantId = Applicants.Id");
            sql.Append($" WHERE Vacancies.Id = @vacancyId");

            Dictionary<string, Stage> stageDict = new Dictionary<string, Stage>();
            Dictionary<string, VacancyCandidate> candidateDict = new Dictionary<string, VacancyCandidate>();
            Dictionary<string, Review> reviewDict = new Dictionary<string, Review>();
            Vacancy cachedVacancy = null;

            IEnumerable<Vacancy> resultAsEnumerable = await connection
                .QueryAsync<Vacancy, Stage, Review, VacancyCandidate, CandidateReview, Applicant, Vacancy>(
                    sql.ToString(),
                    (vacancy, stage, stageReview, candidate, review, applicant) =>
                    {
                        if (cachedVacancy == null)
                        {
                            cachedVacancy = vacancy;
                            cachedVacancy.Stages = new List<Stage>();
                        }

                        if (stage != null)
                        {
                            Stage stageEntry;

                            if (!stageDict.TryGetValue(stage.Id, out stageEntry))
                            {
                                stageEntry = stage;
                                stageEntry.CandidateToStages = new List<CandidateToStage>();
                                stageEntry.ReviewToStages = new List<ReviewToStage>();
                                stageDict.Add(stageEntry.Id, stageEntry);
                                cachedVacancy.Stages.Add(stageEntry);
                            }

                            if (stageReview != null)
                            {
                                Review reviewEntry;

                                if (!reviewDict.TryGetValue(stageReview.Id, out reviewEntry))
                                {
                                    reviewEntry = stageReview;
                                    reviewDict.Add(reviewEntry.Id, reviewEntry);
                                    stageEntry.ReviewToStages.Add(new ReviewToStage { Review = reviewEntry });
                                }
                            }

                            if (candidate != null && applicant != null)
                            {
                                VacancyCandidate candidateEntry;

                                if (!candidateDict.TryGetValue(candidate.Id, out candidateEntry))
                                {
                                    candidateEntry = candidate;
                                    candidateDict.Add(candidateEntry.Id, candidateEntry);
                                    candidateEntry.Reviews = new List<CandidateReview>();
                                    stageEntry.CandidateToStages.Add(new CandidateToStage { Candidate = candidateEntry });
                                }

                                candidateEntry.Applicant = applicant;

                                if (review != null)
                                {
                                    candidateEntry.Reviews.Add(review);
                                }
                            }
                        }

                        return cachedVacancy;
                    },
                     new { vacancyId = @vacancyId },
                     splitOn: "Id,Id,Id,Id");

            Vacancy result = resultAsEnumerable.Distinct().FirstOrDefault();

            if (result == null)
            {
                throw new NotFoundException(typeof(Vacancy), vacancyId);
            }

            await connection.CloseAsync();

            result.Stages = result.Stages.OrderBy(s => s.Index).ToList();

            return result;
        }

        public async Task<Stage> GetByVacancyIdWithFirstIndex(string vacancyId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = $@"SELECT * FROM Stages 
                            WHERE Stages.VacancyId = @vacancyId 
                            AND Stages.[Index]=0";

            return await connection.QueryFirstOrDefaultAsync<Stage>(sql, new { vacancyId = @vacancyId });
        }


        public async Task<IEnumerable<Stage>> GetByVacancyId(string vacancyId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @"SELECT *
                FROM Stages 
                LEFT JOIN Reviews ON EXISTS(
                    SELECT *
                    FROM ReviewToStages
                    WHERE ReviewToStages.StageId = Stages.Id AND ReviewToStages.ReviewId = Reviews.Id
                )
                WHERE Stages.VacancyId = @vacancyId
            ";

            Dictionary<string, Stage> stageDict = new();

            IEnumerable<Stage> stages = await connection.QueryAsync<Stage, Review, Stage>(
                sql,
                (stage, review) =>
                {
                    Stage stageEntry = null;
                    bool tookFromDict = true;

                    if (!stageDict.TryGetValue(stage.Id, out stageEntry))
                    {
                        stageEntry = stage;
                        stageEntry.ReviewToStages = new List<ReviewToStage>();
                        stageDict.Add(stageEntry.Id, stageEntry);
                        tookFromDict = false;
                    }

                    if (review != null)
                    {
                        stageEntry.ReviewToStages.Add(new ReviewToStage { Review = review });
                    }

                    return tookFromDict ? null : stage;
                },
                new { vacancyId = vacancyId },
                splitOn: "Id"
            );

            await connection.CloseAsync();
            return stages.Where(s => s != null).ToList();
        }

        public async Task<Stage> GetWithReviews(string id)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @"SELECT *
                FROM Stages 
                LEFT JOIN ReviewToStages ON ReviewToStages.StageId = Stages.Id
                LEFT JOIN Reviews ON Reviews.Id = ReviewToStages.ReviewId
                WHERE Stages.Id = @id
            ";

            Stage cachedStage = null;

            IEnumerable<Stage> stages = await connection.QueryAsync<Stage, ReviewToStage, Review, Stage>(
                sql,
                (stage, reviewToStage, review) =>
                {
                    if (cachedStage == null)
                    {
                        cachedStage = stage;
                        cachedStage.ReviewToStages = new List<ReviewToStage>();
                    }

                    if (reviewToStage != null && review != null)
                    {
                        reviewToStage.Review = review;
                        cachedStage.ReviewToStages.Add(reviewToStage);
                    }

                    return cachedStage;
                },
                new { id = id },
                splitOn: "Id"
            );

            await connection.CloseAsync();
            return stages.First();
        }

        private class StageIdEqualityComparer : IEqualityComparer<Stage>
        {
            public bool Equals(Stage a, Stage b)
            {
                System.Console.WriteLine(a.Id + " " + b.Id);
                return a.Id == b.Id;
            }

            public int GetHashCode(Stage obj)
            {
                System.Console.WriteLine(obj.Id);
                return obj.GetHashCode();
            }
        }
    }
}
