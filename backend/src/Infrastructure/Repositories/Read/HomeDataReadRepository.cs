using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Infrastructure.Dapper.Interfaces;
using Domain.Entities;
using Domain.Entities.HomeData;
using System.Linq;
using System.Collections.Generic;
using Domain.Interfaces.Read;
using Domain.Enums;

namespace Infrastructure.Repositories.Read
{
    public class HomeDataReadRepository : IHomeDataReadRepository
    {
        protected readonly IConnectionFactory _connectionFactory;

        public HomeDataReadRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<WidgetsData> GetWidgetsDataAsync(string companyId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @$"SELECT Count(*)
                          FROM Applicants AS A
                          WHERE A.CompanyId = @companyId;

                          SELECT V.Id, V.Title, V.IsHot
                          FROM Vacancies AS V
                          WHERE V.CompanyId = @companyId
                                AND NOT EXISTS (SELECT * FROM ArchivedEntities AS AV WHERE AV.EntityType = @entityVacancyType AND AV.EntityId = V.Id)
                          ORDER BY V.CreationDate DESC;

                          SELECT Count(*)
                          FROM Vacancies AS V
                          INNER JOIN Stages AS S ON V.Id = S.VacancyId
                          INNER JOIN CandidateToStages AS CS ON S.Id = CS.StageId
                          WHERE V.CompanyId = @companyId 
                                AND NOT EXISTS (SELECT * FROM ArchivedEntities AS AV WHERE AV.EntityType = @entityVacancyType AND AV.EntityId = V.Id)
		                        AND CS.DateRemoved IS NULL
		                        AND S.[Index] = (SELECT MAX(S2.[Index]) FROM Stages AS S2 WHERE S2.VacancyId = V.Id);

                          SELECT Count(*)
                          FROM Users AS U
                          WHERE U.CompanyId = @companyId;";
            var results = await connection.QueryMultipleAsync(sql, new { companyId = @companyId, entityVacancyType = EntityType.Vacancy });

            WidgetsData widgetsData = new WidgetsData();
            widgetsData.ApplicantCount = await results.ReadSingleAsync<int>();
            widgetsData.Vacancies = (await results.ReadAsync<Vacancy>()).ToList();
            widgetsData.ProcessedCount = await results.ReadSingleAsync<int>();
            widgetsData.HrCount = await results.ReadSingleAsync<int>();

            await connection.CloseAsync();

            return widgetsData;
        }
        public async Task<IEnumerable<HotVacancySummary>> GetHotVacancySummaryAsync(string companyId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @$"SELECT V.Id, V.Title, P.Id AS ProjectId, P.Name AS ProjectName,
                              S.[Index] AS CurrentStageIndex, 
	                          (SELECT MAX(S2.[Index]) FROM Stages AS S2 WHERE S2.VacancyId = V.Id) AS LastStageIndex,
	                          CS.CandidateId AS Candidate,
                              VC.IsSelfApplied
                            FROM Vacancies AS V
                            INNER JOIN Projects AS P ON V.ProjectId = P.Id
                            INNER JOIN Stages AS S ON V.Id = S.VacancyId
                            LEFT JOIN CandidateToStages AS CS ON S.Id = CS.StageId
                            LEFT JOIN VacancyCandidates AS VC ON CS.CandidateId = VC.Id
                            WHERE V.CompanyId = @companyId 
		                      AND V.IsHot = 1
                              AND NOT EXISTS (SELECT * FROM ArchivedEntities AS AV WHERE AV.EntityType = @entityVacancyType AND AV.EntityId = V.Id)
		                      AND CS.DateRemoved IS NULL
                            ORDER BY V.CreationDate DESC;";

            var hotVacancySummary = await connection.QueryAsync<HotVacancySummary>(sql, new { companyId = @companyId, entityVacancyType = EntityType.Vacancy });

            await connection.CloseAsync();

            return hotVacancySummary;
        }
    }
}
