using Application.Common.Exceptions;
using Application.Interfaces;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Abstractions;
using Infrastructure.Dapper.Interfaces;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class ProjectReadRepository : IReadRepository<Project>
    {
        protected readonly IConnectionFactory _connectionFactory;
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly string _tableName = "Projects";

        public ProjectReadRepository(IConnectionFactory connectionFactory, ICurrentUserContext currentUserContext)
        {
            _connectionFactory = connectionFactory;
            _currentUserContext = currentUserContext;
        }

        public async Task<Project> GetAsync(string id)
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @$"
                    SELECT p.*, v.*, AV.*
                    FROM Projects p
                    LEFT OUTER JOIN Vacancies v ON p.Id = v.ProjectId     
                    LEFT OUTER JOIN ArchivedEntities AV ON AV.EntityType = @entityVacancyType AND AV.EntityId = v.Id  
                    WHERE p.Id = @id AND p.CompanyId = @companyId
                    AND NOT EXISTS (SELECT * FROM ArchivedEntities AP WHERE AP.EntityType = @entityProjectType AND AP.EntityId = p.Id)
            ";

            var projectsDictionary = new Dictionary<string, Project>();

            await connection.QueryAsync<Project, Vacancy, ArchivedEntity,Project>(sql, (p, v, archivedVacancy) =>
            {
                Project project;
                if (!projectsDictionary.TryGetValue(p.Id, out project))
                {
                    projectsDictionary.Add(p.Id, project = p);
                }

                if (project.Vacancies == null)
                {
                    project.Vacancies = new List<Vacancy>();
                }

                if (v is not null && archivedVacancy is null)
                {
                    project.Vacancies.Add(v);
                }

                return project;
            },
            new { id = @id, companyId = @companyId, entityProjectType = EntityType.Project, entityVacancyType = EntityType.Vacancy });

            Project project = projectsDictionary.Values.FirstOrDefault();

            if (project == null)
            {
                throw new NotFoundException(typeof(Project), id);
            }

            await connection.CloseAsync();

            return project;
        }

        public async Task<Project> GetByPropertyAsync(string property, string propertyValue)
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            using var connection = _connectionFactory.GetSqlConnection();
            string sql = @$"SELECT * FROM {_tableName} P
                            WHERE P.[{property}] = @propertyValue 
                            AND P.CompanyId = @companyId
                            AND NOT EXISTS (SELECT * FROM ArchivedEntities AP WHERE AP.EntityType = @entityProjectType AND AP.EntityId = P.Id)";
            return await connection.QueryFirstOrDefaultAsync<Project>(sql, new
            {
                propertyValue = @propertyValue,
                companyId = @companyId,
                entityProjectType = EntityType.Project
            });
        }

        public async Task<IEnumerable<Project>> GetEnumerableAsync()
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @$"
                    SELECT p.*, v.*, AV.*
                    FROM Projects p
                    LEFT OUTER JOIN Vacancies v ON p.Id = v.ProjectId     
                    LEFT OUTER JOIN ArchivedEntities AV ON AV.EntityType = @entityVacancyType AND AV.EntityId = v.Id  
                    WHERE p.CompanyId = @companyId
                    AND NOT EXISTS (SELECT * FROM ArchivedEntities AS AP WHERE AP.EntityType = @entityProjectType AND AP.EntityId = p.Id)
                    ORDER BY p.CreationDate DESC;;";

            var projectsDictionary = new Dictionary<string, Project>();

            await connection.QueryAsync<Project, Vacancy, ArchivedEntity, Project>(sql, (p, v, archivedVacancy) =>
            {
                Project project;
                if (!projectsDictionary.TryGetValue(p.Id, out project))
                {
                    projectsDictionary.Add(p.Id, project = p);
                }

                if (project.Vacancies == null)
                {
                    project.Vacancies = new List<Vacancy>();
                }

                if (v is not null && archivedVacancy is null)
                {
                    project.Vacancies.Add(v);
                }

                return project;
            }, new { companyId = @companyId, entityProjectType = EntityType.Project, entityVacancyType = EntityType.Vacancy });

            var projects = projectsDictionary.Values;

            await connection.CloseAsync();

            return projects;
        }

        public async Task<IEnumerable<Project>> GetEnumerableByPropertyAsync(string property, string propertyValue)
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            using var connection = _connectionFactory.GetSqlConnection();
            string sql = @$"SELECT * FROM {_tableName} P
                            WHERE P.[{property}] = @propertyValue 
                            AND P.CompanyId = @companyId
                            AND NOT EXISTS (SELECT * FROM ArchivedEntities AP WHERE AP.EntityType = @entityProjectType AND AP.EntityId = P.Id)";
            return await connection.QueryAsync<Project>(sql, new
            {
                propertyValue = @propertyValue,
                companyId = @companyId,
                entityProjectType = EntityType.Project
            });
        }
    }
}