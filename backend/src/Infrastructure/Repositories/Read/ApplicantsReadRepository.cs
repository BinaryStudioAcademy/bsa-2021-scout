using Dapper;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Dapper.Interfaces;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Domain.Interfaces.Read;
using System.Collections.Generic;
using Application.Applicants.Dtos;
using Application.Interfaces;
using Application.Common.Exceptions;

namespace Infrastructure.Repositories.Read
{
    public class ApplicantsReadRepository : ReadRepository<Applicant>, IApplicantsReadRepository
    {
        protected readonly ICurrentUserContext _currentUserContext;
        public ApplicantsReadRepository(IConnectionFactory connectionFactory, ICurrentUserContext currentUserContext)
            : base("Applicants", connectionFactory)
        {
            _currentUserContext = currentUserContext;
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

        public async Task<IEnumerable<(Applicant, bool)>> GetApplicantsWithAppliedMark(string vacancyId)
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = @$"SELECT AllApplicants.*, CASE WHEN Applied.[Index] IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END IsApplied FROM Applicants AS AllApplicants
                           LEFT OUTER JOIN
                           (SELECT VacancyCandidates.ApplicantId, Stages.[Index]
                           From Stages 
                           LEFT OUTER JOIN CandidateToStages ON CandidateToStages.StageId = Stages.Id AND Stages.[Index]=0 
                           LEFT OUTER JOIN VacancyCandidates ON CandidateToStages.CandidateId = VacancyCandidates.Id
                           WHERE Stages.VacancyId='{vacancyId}') AS Applied ON AllApplicants.Id=Applied.ApplicantId
                           WHERE AllApplicants.CompanyId = '{companyId}'";

            var result = await connection.QueryAsync<Applicant, bool, (Applicant, bool)>(sql,
                (applicant, isApplied) =>
                {
                    (Applicant, bool) pair;

                    pair.Item1 = applicant;
                    pair.Item2 = isApplied;

                    return pair;
                },
            splitOn: "IsApplied");

            await connection.CloseAsync();

            return result;
        }

        public async Task<Applicant> GetByCompanyIdAsync(string id)
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @$"
                    SELECT * FROM Applicants
                    WHERE Applicants.Id='{id}'
                    AND Applicants.CompanyId='{companyId}'";

            var applicant = await connection.QueryFirstOrDefaultAsync<Applicant>(sql);

            if (applicant == null)
            {
                throw new NotFoundException(typeof(Applicant), id);
            }

            await connection.CloseAsync();

            return applicant;
        }
    }
}