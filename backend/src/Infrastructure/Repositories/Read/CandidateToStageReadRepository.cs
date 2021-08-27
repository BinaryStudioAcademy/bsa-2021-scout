
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

        public async Task<IEnumerable<CandidateToStage>> GetRecentAsync(string userId, int page = 1)
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
                OFFSET @skip ROWS
                FETCH NEXT @take ROWS ONLY
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

            await connection.CloseAsync();

            return candidateToStages;
        }
    }
}
