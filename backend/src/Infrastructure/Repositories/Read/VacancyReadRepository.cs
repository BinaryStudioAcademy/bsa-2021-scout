using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Interfaces;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Repositories.Read
{
    public class VacancyReadRepository : ReadRepository<Vacancy> , IVacancyReadRepository
    {
        protected readonly ICurrentUserContext _currentUserContext;
        public VacancyReadRepository(IConnectionFactory connectionFactory, ICurrentUserContext currentUserContext) : base("Vacancies", connectionFactory) {
            _currentUserContext = currentUserContext;
        }

        public async Task<Vacancy> GetByCompanyIdAsync(string id)
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @$"
                    SELECT * FROM Vacancies
                    WHERE Vacancies.Id='{id}'
                    AND Vacancies.CompanyId='{companyId}'";

            var vacancy = await connection.QueryFirstOrDefaultAsync<Vacancy>(sql);

            if (vacancy == null)
            {
                throw new NotFoundException(typeof(Vacancy), id);
            }

            await connection.CloseAsync();

            return vacancy;
        }

        public async Task<IEnumerable<Vacancy>> GetEnumerableNotAppliedByApplicantId(string applicantId)
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @$"(SELECT Vacancies.* FROM Vacancies
							JOIN Projects ON Projects.Id=Vacancies.ProjectId
							WHERE NOT EXISTS (SELECT * FROM ArchivedEntities AS AP WHERE AP.EntityType = @entityProjectType AND AP.EntityId = Projects.Id)
                              AND NOT EXISTS (SELECT * FROM ArchivedEntities AS AV WHERE AV.EntityType = @entityVacancyType AND AV.EntityId = Vacancies.Id))
                            EXCEPT
                            (SELECT V.* FROM Vacancies AS V
                            LEFT OUTER JOIN Stages AS S ON S.VacancyId=V.Id
                            LEFT OUTER JOIN CandidateToStages AS CS ON CS.StageId = S.Id AND (S.[Index]=1 OR S.[Index]=0)
                            LEFT OUTER JOIN VacancyCandidates AS VC ON CS.CandidateId = VC.Id
                            WHERE VC.ApplicantId = @applicantId
                            AND V.CompanyId = @companyId)";

            var vacancies = await connection.QueryAsync<Vacancy>(sql, new {
                applicantId = @applicantId,
                companyId = @companyId,
                entityProjectType = EntityType.Project,
                entityVacancyType = EntityType.Vacancy
            });

            await connection.CloseAsync();

            return vacancies;
        }

    }
}
