using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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

        public async Task ReplaceForCandidate(string candidateId, string newStageId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = @"
                SELECT *
                FROM CandidateToStages
                WHERE (
                    CandidateToStages.DateRemoved IS NULL AND
                    CandidateToStages.CandidateId = @id
                )
            ";

            CandidateToStage newEntity = new CandidateToStage
            {
                CandidateId = candidateId,
                StageId = newStageId,
                DateAdded = DateTime.UtcNow,
            };

            await connection.OpenAsync();

            CandidateToStage oldEntity = await connection
                .QueryFirstAsync<CandidateToStage>(sql.ToString(), new { id = candidateId });

            await connection.CloseAsync();

            oldEntity.DateRemoved = DateTime.UtcNow;

            _context.CandidateToStages.Add(newEntity);
            _context.CandidateToStages.Update(oldEntity);

            await _context.SaveChangesAsync();
        }
    }
}
