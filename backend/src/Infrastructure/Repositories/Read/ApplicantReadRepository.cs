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
using Domain.Enums;

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
            await connection.OpenAsync();

            var query = @"SELECT fi.* FROM Applicants a
                          INNER JOIN FileInfos fi ON a.CvFileInfoId = fi.Id
                          WHERE a.Id = @applicantId;";

            var fileInfo = await connection.QueryFirstOrDefaultAsync<FileInfo>(query, new { applicantId = applicantId });
            await connection.CloseAsync();

            if (fileInfo == null)
            {
                throw new ApplicantCvNotFoundException(applicantId);
            }

            return fileInfo;
        }

        public async Task<FileInfo> GetPhotoFileInfoAsync(string applicantId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            var query = @"SELECT fi.* FROM Applicants a
                          INNER JOIN FileInfos fi ON a.PhotoFileInfoId = fi.Id
                          WHERE a.Id = @applicantId";

            var fileInfo = await connection.QueryFirstOrDefaultAsync<FileInfo>(query, new { applicantId = applicantId });
            await connection.CloseAsync();

            if (fileInfo == null)
            {
                throw new ApplicantPhotoNotFoundException(applicantId);
            }

            return fileInfo;
        }

        public async Task<IEnumerable<Applicant>> GetCompanyApplicants()
        {
            var companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = @$"SELECT a.*, fi.* FROM {_tableName} a
                           LEFT JOIN FileInfos fi ON a.CvFileInfoId = fi.Id
                           WHERE a.CompanyId = @companyId
                           ORDER BY a.CreationDate DESC;";

            await connection.OpenAsync();
            var entities = await connection.QueryAsync<Applicant, FileInfo, Applicant>(sql,
            (a, fi) =>
            {
                a.CvFileInfo = fi;
                return a;
            },
            splitOn: "Id,Id",
            param: new { companyId });

            await connection.CloseAsync();

            return entities;
        }

        public async Task<IEnumerable<ApplicantVacancyInfo>> GetApplicantVacancyInfoListAsync(string applicantId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = "SELECT Vacancies.Id, Vacancies.Title, Stages.Id, Stages.Name, " +
                         "Projects.Id, Projects.Name, CandidateToStages.StageId, VacancyCandidates.Id FROM Vacancies " +
                         "JOIN Stages ON Vacancies.Id = Stages.VacancyId " +
                         "JOIN Projects ON Vacancies.ProjectId = Projects.Id " +
                         "JOIN CandidateToStages ON CandidateToStages.StageId = Stages.Id " +
                         "JOIN VacancyCandidates ON CandidateToStages.CandidateId = VacancyCandidates.Id " +
                         "WHERE VacancyCandidates.ApplicantId = @applicantId " +
                         "AND NOT EXISTS (SELECT * FROM ArchivedEntities AS AV WHERE AV.EntityType = @entityVacancyType AND AV.EntityId = Vacancies.Id);";

            await connection.OpenAsync();

            var applicantVacancyInfos = await connection
                .QueryAsync<Vacancy, Stage, Project, CandidateToStage, VacancyCandidate, ApplicantVacancyInfo>(sql,
                    (v, s, p, cs, vc) =>
                    {
                        return new ApplicantVacancyInfo()
                        {
                            Title = v.Title,
                            Stage = s.Name,
                            Project = p.Name,
                        };
                    },
                    new { applicantId = @applicantId, entityVacancyType = EntityType.Vacancy },
                    splitOn: "Id,Id,StageId,Id"
                );

            await connection.CloseAsync();

            return applicantVacancyInfos;
        }

        public async Task<Applicant> GetByIdAsync(string applicantId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = @$"SELECT a.*, fi.*, pfi.* FROM {_tableName} a
                            LEFT JOIN FileInfos fi ON a.CvFileInfoId = fi.Id
                            LEFT JOIN FileInfos pfi ON a.PhotoFileInfoId = pfi.Id
                            WHERE a.Id = @applicantId";

            await connection.OpenAsync();
            var entities = await connection.QueryAsync<Applicant, FileInfo, FileInfo, Applicant>(sql,
            (a, fi, pfi) =>
            {
                a.CvFileInfo = fi;
                a.PhotoFileInfo = pfi;
                return a;
            },
            splitOn: "Id,Id",
            param: new { applicantId });

            await connection.CloseAsync();

            var applicant = entities.FirstOrDefault();

            if (applicant == null)
            {
                throw new NotFoundException(typeof(Applicant), applicantId);
            }

            return applicant;
        }

        public async Task<IEnumerable<(Applicant, bool)>> GetApplicantsWithAppliedMark(string vacancyId)
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = @$"SELECT DISTINCT AllApplicants.*, CASE WHEN Applied.[Index] IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END IsApplied FROM Applicants AS AllApplicants
                           LEFT OUTER JOIN
                           (SELECT VacancyCandidates.ApplicantId, Stages.[Index]
                           From Stages 
                           LEFT OUTER JOIN CandidateToStages ON CandidateToStages.StageId = Stages.Id AND (Stages.[Index]=0 OR Stages.[Index]=1)
                           LEFT OUTER JOIN VacancyCandidates ON CandidateToStages.CandidateId = VacancyCandidates.Id
                           WHERE Stages.VacancyId = @vacancyId) AS Applied ON AllApplicants.Id=Applied.ApplicantId
                           WHERE AllApplicants.CompanyId = @companyId
                           order by AllApplicants.firstName, AllApplicants.lastName";

            var result = await connection.QueryAsync<Applicant, bool, (Applicant, bool)>(sql,
                (applicant, isApplied) =>
                {
                    (Applicant, bool) pair;

                    pair.Item1 = applicant;
                    pair.Item2 = isApplied;

                    return pair;
                }, new
                {
                    vacancyId = @vacancyId,
                    companyId = @companyId
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
                    WHERE Applicants.Id = @Id
                    AND Applicants.CompanyId = @companyId";

            var applicant = await connection.QueryFirstOrDefaultAsync<Applicant>(sql,
            new
            {
                Id = id,
                companyId = @companyId
            });

            if (applicant == null)
            {
                throw new NotFoundException(typeof(Applicant), id);
            }

            await connection.CloseAsync();

            return applicant;
        }

        public override async Task<IEnumerable<Applicant>> GetEnumerableAsync()
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            string sql = @$"SELECT * FROM {_tableName} 
                            WHERE Applicants.CompanyId = @companyId";

            var entities = await connection.QueryAsync<Applicant>(sql,
                new
                {
                    companyId = @companyId
                });
            await connection.CloseAsync();

            return entities;
        }
    }
}
