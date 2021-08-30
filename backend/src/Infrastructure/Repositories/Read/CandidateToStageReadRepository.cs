
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Domain.Interfaces.Abstractions;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class CandidateToStageReadRepository
        : ReadRepository<CandidateToStage>, ICandidateToStageReadRepository
    {
        private const int RECENT_PAGE_SIZE = 20;
        private readonly IReadRepository<User> _userRepository;

        public CandidateToStageReadRepository(
            IConnectionFactory connectionFactory,
            IReadRepository<User> userRepository
        ) : base("CandidateToStages", connectionFactory)
        {
            _userRepository = userRepository;
        }

        public async Task<(IEnumerable<CandidateToStage>, bool)> GetRecentAsync(string userId, int page = 1)
        {
            User user = await _userRepository.GetAsync(userId);

            SqlConnection connection = _connectionFactory.GetSqlConnection();
            int skip = (page - 1) * RECENT_PAGE_SIZE;

            string sql = @"
                SELECT
                    CandidateToStages.*,
                    Stages.Id,
                    Stages.Name,
                    VacancyCandidates.Id,
                    Applicants.Id,
                    Applicants.FirstName,
                    Applicants.LastName,
                    Users.Id,
                    Users.FirstName,
                    Users.LastName
                FROM CandidateToStages
                LEFT JOIN Stages ON Stages.Id = CandidateToStages.StageId
                LEFT JOIN VacancyCandidates ON VacancyCandidates.Id = CandidateToStages.CandidateId
                LEFT JOIN Applicants ON Applicants.Id = VacancyCandidates.ApplicantId
                LEFT JOIN Users ON Users.Id = CandidateToStages.MoverId
                WHERE Users.CompanyId = @companyId
                ORDER BY CandidateToStages.DateAdded DESC
                OFFSET @skip ROWS
                FETCH NEXT @take ROWS ONLY
            ";

            string countSql = @"
                SELECT COUNT(*)
                FROM CandidateToStages
                LEFT JOIN Users ON Users.Id = CandidateToStages.MoverId
                WHERE Users.CompanyId = @companyId
            ";

            await connection.OpenAsync();

            IEnumerable<CandidateToStage> candidateToStages = await connection
                .QueryAsync<CandidateToStage, Stage, VacancyCandidate, Applicant, User, CandidateToStage>(
                    sql,
                    (candidateToStage, stage, candidate, applicant, user) =>
                    {
                        candidateToStage.Stage = stage;
                        candidateToStage.Candidate = candidate;
                        candidateToStage.Candidate.Applicant = applicant;
                        candidateToStage.Mover = user;

                        return candidateToStage;
                    },
                    new { skip = skip, take = RECENT_PAGE_SIZE, companyId = user.CompanyId },
                    splitOn: "Id,Id,Id,Id"
                );

            int count = await connection.QueryFirstAsync<int>(countSql, new { companyId = user.CompanyId });

            await connection.CloseAsync();

            return (candidateToStages, (skip + RECENT_PAGE_SIZE) >= count);
        }

        public async Task<IEnumerable<CandidateToStage>> GetRecentForApplicantAsync(string applicantId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = @"
                SELECT
                    CandidateToStages.*,
                    Stages.Id,
                    Stages.Name,
                    Vacancies.Id,
                    Vacancies.Title,
                    Users.Id,
                    Users.FirstName,
                    Users.LastName
                FROM CandidateToStages
                LEFT JOIN Stages ON Stages.Id = CandidateToStages.StageId
                LEFT JOIN Vacancies ON Vacancies.Id = Stages.VacancyId
                LEFT JOIN Users ON Users.Id = CandidateToStages.MoverId
                LEFT JOIN Applicants ON EXISTS(
                    SELECT Id
                    FROM VacancyCandidates
                    WHERE (
                        VacancyCandidates.ApplicantId = Applicants.Id AND
                        CandidateToStages.CandidateId = VacancyCandidates.Id
                    )
                )
                WHERE Applicants.Id = @applicantId
            ";

            IEnumerable<CandidateToStage> candidateToStages = await connection
                .QueryAsync<CandidateToStage, Stage, Vacancy, User, CandidateToStage>(
                    sql,
                    (cts, stage, vacancy, user) =>
                    {
                        cts.Stage = stage;
                        cts.Stage.Vacancy = vacancy;
                        cts.Mover = user;

                        return cts;
                    },
                    new { applicantId = applicantId },
                    splitOn: "Id,Id,Id"
                );

            await connection.CloseAsync();

            return candidateToStages;
        }
    }
}
