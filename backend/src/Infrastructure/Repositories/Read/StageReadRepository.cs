using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Interfaces.Read;
using Domain.Entities;
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

            string vacancySql = $@"
                SELECT Id, Title
                FROM Vacancies
                WHERE Id = @id;
            ";

            Vacancy vacancy = await connection.QueryFirstAsync<Vacancy>(vacancySql, new { id = vacancyId });

            string stagesSql = $@"
                SELECT *
                FROM {_tableName} Stage
                WHERE VacancyId = @vacancyId
                ORDER BY Stage.[Index] ASC
            ";

            IEnumerable<Stage> stagesRaw = await connection.QueryAsync<Stage>(stagesSql, new { vacancyId = vacancyId });

            string candidatesSql = $@"
                SELECT *
                FROM VacancyCandidates
                WHERE VacancyCandidates.StageId IN ({string.Join(", ", stagesRaw.Select(s => $"'{s.Id}'"))})
            ";

            IEnumerable<VacancyCandidate> candidatesRaw = await connection.QueryAsync<VacancyCandidate>(candidatesSql);

            string applicantsSql = $@"
                SELECT *
                FROM Applicants
                WHERE Applicants.Id IN ({string.Join(", ", candidatesRaw.Select(c => $"'{c.ApplicantId}'"))})
            ";

            IEnumerable<Applicant> applicants = await connection.QueryAsync<Applicant>(applicantsSql);

            await connection.CloseAsync();

            IEnumerable<VacancyCandidate> candidates = candidatesRaw.Join<VacancyCandidate, Applicant, string, VacancyCandidate>(
                applicants,
                c => c.ApplicantId,
                a => a.Id,
                (candidate, applicant) => new VacancyCandidate
                {
                    Id = candidate.Id,
                    FirstContactDate = candidate.FirstContactDate,
                    SecondContactDate = candidate.SecondContactDate,
                    ThirdContactDate = candidate.ThirdContactDate,
                    SalaryExpectation = candidate.SalaryExpectation,
                    ContactedBy = candidate.ContactedBy,
                    Comments = candidate.Comments,
                    Experience = candidate.Experience,
                    StageId = candidate.StageId,
                    ApplicantId = candidate.ApplicantId,
                    Applicant = applicant,
                }
            );

            IEnumerable<Stage> stages = stagesRaw.GroupJoin<Stage, VacancyCandidate, string, Stage>(
                candidates,
                s => s.Id,
                c => c.StageId,
                (stage, stageCandidates) => new Stage
                {
                    Id = stage.Id,
                    Name = stage.Name,
                    Type = stage.Type,
                    Index = stage.Index,
                    IsReviewable = stage.IsReviewable,
                    VacancyId = stage.VacancyId,
                    Candidates = stageCandidates.ToList(),
                }
            );

            return new Vacancy
            {
                Id = vacancy.Id,
                Title = vacancy.Title,
                Stages = stages.ToList(),
            };
        }
    }
}
