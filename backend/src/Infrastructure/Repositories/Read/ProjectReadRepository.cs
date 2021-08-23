using Application.Common.Exceptions;
using Application.Interfaces;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
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
                    SELECT p.*, v.*
                    FROM Projects p
                    LEFT OUTER JOIN Vacancies v ON p.Id = v.ProjectId     
                    WHERE p.Id = @id AND p.IsDeleted = 0 AND p.CompanyId = '{companyId}'
            ";

            var projectsDictionary = new Dictionary<string, Project>();

            await connection.QueryAsync<Project, Vacancy, Project>(sql, (p, v) =>
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

                if (v != null)
                {
                    project.Vacancies.Add(v);
                }

                return project;
            },
            new { id = @id });

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
            string sql = $"SELECT * FROM {_tableName} WHERE [{property}] = @propertyValue AND IsDeleted = 0 AND CompanyId = '{companyId}'";
            return await connection.QueryFirstOrDefaultAsync<Project>(sql, new { propertyValue });
        }

        public async Task<IEnumerable<Project>> GetEnumerableAsync()
        {
            string companyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @$"
                    SELECT p.*, v.*
                    FROM Projects p
                    LEFT OUTER JOIN Vacancies v ON p.Id = v.ProjectId     
                    WHERE p.IsDeleted = 0 AND p.CompanyId = '{companyId}'
            ";

            var projectsDictionary = new Dictionary<string, Project>();

            await connection.QueryAsync<Project, Vacancy, Project>(sql, (p, v) =>
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

                if (v != null)
                {
                    project.Vacancies.Add(v);
                }

                return project;
            });

            var projects = projectsDictionary.Values;

            await connection.CloseAsync();

            return projects;
        }
    }
}