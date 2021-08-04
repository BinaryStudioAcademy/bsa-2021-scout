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
            sql.Append(" VacancyCandidates.*,");
            sql.Append(" Applicants.*");
            sql.Append(" FROM Vacancies");
            sql.Append(" LEFT JOIN Stages ON Stages.VacancyId = Vacancies.Id");
            sql.Append(" LEFT JOIN VacancyCandidates ON VacancyCandidates.StageId = Stages.Id");
            sql.Append(" LEFT JOIN Applicants ON VacancyCandidates.ApplicantId = Applicants.Id");
            sql.Append($" WHERE Vacancies.Id = '{vacancyId}'");

            Dictionary<string, Stage> stageDict = new Dictionary<string, Stage>();
            Vacancy cachedVacancy = null;

            IEnumerable<Vacancy> resultAsEnumerable = await connection
                .QueryAsync<Vacancy, Stage, VacancyCandidate, Applicant, Vacancy>(
                    sql.ToString(),
                    (vacancy, stage, candidate, applicant) =>
                    {
                        if (cachedVacancy == null)
                        {
                            cachedVacancy = vacancy;
                            cachedVacancy.Stages = new List<Stage>();
                        }

                        if (candidate != null && applicant != null)
                        {
                            candidate.Applicant = applicant;
                        }

                        Stage stageEntry;

                        if (!stageDict.TryGetValue(stage.Id, out stageEntry))
                        {
                            stageEntry = stage;
                            stageEntry.Candidates = new List<VacancyCandidate>();
                            stageDict.Add(stageEntry.Id, stageEntry);
                            cachedVacancy.Stages.Add(stageEntry);
                        }

                        if (candidate != null)
                        {
                            stageEntry.Candidates.Add(candidate);
                        }

                        return cachedVacancy;
                    },
                    splitOn: "Id,Id,Id"
                );

            Vacancy result = resultAsEnumerable.Distinct().FirstOrDefault();

            if (result == null)
            {
                throw new NotFoundException(typeof(Vacancy), vacancyId);
            }

            await connection.CloseAsync();

            result.Stages = result.Stages.OrderBy(s => s.Index).ToList();

            return result;
        }
    }
}
