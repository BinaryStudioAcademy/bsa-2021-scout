using Dapper;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Dapper.Interfaces;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Domain.Interfaces.Read;
using System.Collections.Generic;
using Application.Interfaces;

namespace Infrastructure.Repositories.Read
{
    public class ApplicantsReadRepository : ReadRepository<Applicant>, IApplicantsReadRepository
    {
        private readonly ICurrentUserContext _currentUserContext;

        public ApplicantsReadRepository(IConnectionFactory connectionFactory, ICurrentUserContext currentUserContext)
            : base("Applicants", connectionFactory)
        {
            _currentUserContext = currentUserContext;
        }

        public async Task<IEnumerable<Applicant>> GetCompanyApplicants()
        {
            var companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;
            
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            string sql = $"SELECT * FROM {_tableName} WHERE [CompanyId] = @companyId";
            var entities = await connection.QueryAsync<Applicant>(sql, new { companyId });
            await connection.CloseAsync();
            
            return entities;
        }

        public async Task<IEnumerable<ApplicantVacancyInfo>> GetApplicantVacancyInfoListAsync(string applicantId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = "SELECT Vacancies.Id, Vacancies.Title, Stages.Id, Stages.Name, " +
                         "CandidateToStages.StageId, VacancyCandidates.Id FROM Vacancies " +
                         "JOIN Stages ON Vacancies.Id = Stages.VacancyId " +
                         "JOIN CandidateToStages ON CandidateToStages.StageId = Stages.Id " +
                         "JOIN VacancyCandidates ON CandidateToStages.CandidateId = VacancyCandidates.Id " +
                         $"WHERE VacancyCandidates.ApplicantId = \'{applicantId}\'";

            await connection.OpenAsync();
            var applicantVacancyInfos = await connection.QueryAsync<Vacancy, Stage, CandidateToStage, VacancyCandidate, ApplicantVacancyInfo>(sql,
            (v, s, cs, vc) =>
            {
                return new ApplicantVacancyInfo()
                {
                    Title = v.Title,
                    Stage = s.Name
                };
            },
            splitOn: "Id,StageId,Id");
            await connection.CloseAsync();

            return applicantVacancyInfos;
        }
    }
}