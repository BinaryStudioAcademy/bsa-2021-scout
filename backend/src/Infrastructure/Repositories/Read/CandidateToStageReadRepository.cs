
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

        public async Task<CandidateToStage> GetCurrentForCandidateByVacancyAsync(string candidateId, string vacancyId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = @"
                SELECT CandidateToStages.*
                FROM CandidateToStages
                LEFT JOIN Stages ON Stages.Id = CandidateToStages.StageId
                WHERE (
                    Stages.VacancyId = @vacancyId AND
                    CandidateToStages.DateRemoved IS NULL AND
                    CandidateToStages.CandidateId = @id
                )
            ";

            await connection.OpenAsync();

            var result = await connection
                .QueryFirstAsync<CandidateToStage>(sql.ToString(), new { id = candidateId, vacancyId = vacancyId });

            await connection.CloseAsync();

            return result;
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
                    Vacancies.Id,
                    Vacancies.Title,
                    VacancyCandidates.Id,
                    Applicants.Id,
                    Applicants.FirstName,
                    Applicants.LastName,
                    Users.Id,
                    Users.FirstName,
                    Users.LastName
                FROM CandidateToStages
                LEFT JOIN Stages ON Stages.Id = CandidateToStages.StageId
                LEFT JOIN Vacancies ON Vacancies.Id = Stages.VacancyId
                LEFT JOIN VacancyCandidates ON VacancyCandidates.Id = CandidateToStages.CandidateId
                LEFT JOIN Applicants ON Applicants.Id = VacancyCandidates.ApplicantId
                LEFT JOIN Users ON Users.Id = CandidateToStages.MoverId
                WHERE Users.CompanyId = @companyId AND Stages.[Index] > 0
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
                .QueryAsync<CandidateToStage, Stage, Vacancy, VacancyCandidate, Applicant, User, CandidateToStage>(
                    sql,
                    (candidateToStage, stage, vacancy, candidate, applicant, user) =>
                    {
                        candidateToStage.Stage = stage;
                        candidateToStage.Stage.Vacancy = vacancy;
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
                    Projects.Id,
                    Projects.Name,
                    Users.Id,
                    Users.FirstName,
                    Users.LastName
                FROM CandidateToStages
                LEFT JOIN Stages ON Stages.Id = CandidateToStages.StageId
                LEFT JOIN Vacancies ON Vacancies.Id = Stages.VacancyId
                LEFT JOIN Projects ON Projects.Id = Vacancies.ProjectId
                LEFT JOIN Users ON Users.Id = CandidateToStages.MoverId
                LEFT JOIN VacancyCandidates ON VacancyCandidates.Id = CandidateToStages.CandidateId
                WHERE VacancyCandidates.ApplicantId = @applicantId AND Stages.[Index] > 0
            ";

            await connection.OpenAsync();

            IEnumerable<CandidateToStage> candidateToStages = await connection
                .QueryAsync<CandidateToStage, Stage, Vacancy, Project, User, CandidateToStage>(
                    sql,
                    (candidateToStage, stage, vacancy, project, user) =>
                    {
                        candidateToStage.Stage = stage;
                        candidateToStage.Stage.Vacancy = vacancy;
                        candidateToStage.Stage.Vacancy.Project = project;
                        candidateToStage.Mover = user;

                        return candidateToStage;
                    },
                    new { applicantId = applicantId },
                    splitOn: "Id,Id,Id"
                );

            await connection.CloseAsync();

            return candidateToStages;
        }
    }
}
