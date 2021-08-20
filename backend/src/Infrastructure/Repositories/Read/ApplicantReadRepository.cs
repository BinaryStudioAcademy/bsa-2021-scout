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
using System;
using Application.Common.Exceptions.Applicants;
using Application.Interfaces;

namespace Infrastructure.Repositories.Read
{
    public class ApplicantReadRepository : ReadRepository<Applicant>, IApplicantReadRepository
    {
        private readonly ICurrentUserContext _currentUserContext;

        public ApplicantReadRepository(IConnectionFactory connectionFactory, ICurrentUserContext currentUserContext) : base("Applicants", connectionFactory)
        {
            _currentUserContext = currentUserContext;
        }

        public async Task<FileInfo> GetCvFileInfoAsync(string applicantId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            var query = @"SELECT fi.* FROM Applicants a
                          INNER JOIN FileInfos fi ON a.CvFileInfoId = fi.Id
                          WHERE a.Id = @applicantId;";

            var fileInfo = await connection.QueryFirstAsync<FileInfo>(query, new { applicantId = applicantId });

            if (fileInfo == null)
            {
                throw new ApplicantCvNotFoundException(applicantId);
            }

            await connection.CloseAsync();

            return fileInfo;
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

        public async Task<Applicant> GetByIdAsync(string applicantId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();
            
            string sql = $"SELECT * FROM {_tableName} WHERE Id = @id";

            await connection.OpenAsync();

            var applicant = await connection.QueryFirstOrDefaultAsync<Applicant>(sql, new { id = applicantId });

            if (applicant == null)
            {
                throw new NotFoundException(typeof(Applicant), applicantId);
            }

            await connection.CloseAsync();

            try
            {
                // TODO: move to one query
                var cvFileInfo = await GetCvFileInfoAsync(applicantId);
                applicant.CvFileInfo = cvFileInfo;
            } catch (Exception _)
            {

            }

            return applicant;
        }
    }
}
