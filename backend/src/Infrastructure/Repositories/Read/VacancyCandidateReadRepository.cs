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

        public async Task<VacancyCandidate> GetFull(string id)
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
            sql.Append($" WHERE VacancyCandidates.Id = {id}");

            IEnumerable<VacancyCandidate> resultAsArray = await connection
                .QueryAsync<VacancyCandidate, Applicant, Stage, VacancyCandidate>(
                    sql.ToString(),
                    (candidate, applicant, stage) =>
                    {
                        candidate.Applicant = applicant;
                        // candidate.Stage = stage;

                        return candidate;
                    },
                    splitOn: "Id,Id"
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
