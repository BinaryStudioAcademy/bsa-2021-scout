using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Write;
using Infrastructure.EF;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Write
{
    public class CandidateToStageWriteRepository : WriteRepository<CandidateToStage>, ICandidateToStageWriteRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public CandidateToStageWriteRepository(
            ApplicationDbContext context,
            IConnectionFactory connectionFactory
        ) : base(context)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task ReplaceForCandidate(string userId, string candidateId, string vacancyId, string newStageId)
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

            CandidateToStage newEntity = new CandidateToStage
            {
                CandidateId = candidateId,
                StageId = newStageId,
                MoverId = userId,
                DateAdded = DateTime.UtcNow,
            };

            await connection.OpenAsync();

            CandidateToStage oldEntity = await connection
                .QueryFirstAsync<CandidateToStage>(sql.ToString(), new { id = candidateId, vacancyId = vacancyId });

            await connection.CloseAsync();

            oldEntity.DateRemoved = DateTime.UtcNow;
            await UpdateAsync(oldEntity);

            await CreateAsync(newEntity);
        }
    }
}
