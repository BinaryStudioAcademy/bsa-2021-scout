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

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT *");
            sql.Append(" FROM CandidateToStages");
            sql.Append($" WHERE CandidateToStages.DateRemoved IS NULL AND CandidateToStages.CandidateId = '{candidateId}'");

            CandidateToStage newEntity = new CandidateToStage
            {
                CandidateId = candidateId,
                StageId = newStageId,
                DateAdded = DateTime.UtcNow,
            };

            CandidateToStage oldEntity = await connection.QueryFirstAsync<CandidateToStage>(sql.ToString());
            oldEntity.DateRemoved = DateTime.UtcNow;

            await connection.CloseAsync();

            _context.Add(newEntity);
            _context.Update(oldEntity);

            await _context.SaveChangesAsync();
        }
    }
}
