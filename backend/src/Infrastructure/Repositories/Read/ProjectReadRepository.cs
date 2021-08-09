using Application.Common.Exceptions;
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
        protected readonly string _tableName = "Projects";

        public ProjectReadRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Project> GetAsync(string id)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = $"SELECT * FROM {_tableName} WHERE Id = @id AND IsDeleted = 0";
            Project project = await connection.QueryFirstOrDefaultAsync<Project>(sql, new { id = id });

            if (project == null)
            {
                throw new NotFoundException(typeof(Project), id);
            }

            string sqlVacancies = $"SELECT * FROM Vacancies WHERE ProjectId = '{project.Id}'";
            project.Vacancies = (await connection.QueryAsync<Vacancy>(sqlVacancies)).ToList();

            await connection.CloseAsync();

            return project;
        }

        public async Task<Project> GetByPropertyAsync(string property, string propertyValue)
        {
            using var connection = _connectionFactory.GetSqlConnection();
            string sql = $"SELECT * FROM {_tableName} WHERE [{property}] = @propertyValue AND IsDeleted = 0";
            return await connection.QueryFirstOrDefaultAsync<Project>(sql, new { propertyValue });
        }

        public async Task<IEnumerable<Project>> GetEnumerableAsync()
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            string sql = $"SELECT * FROM {_tableName} WHERE IsDeleted = 0";

            IEnumerable<Project> projects = await connection.QueryAsync<Project>(sql);

            foreach (var project in projects)
            {
                string sqlVacancies = $"SELECT * FROM Vacancies WHERE ProjectId = '{project.Id}'";
                project.Vacancies = (await connection.QueryAsync<Vacancy>(sqlVacancies)).ToList();
            }

            await connection.CloseAsync();

            return projects;
        }
    }
}
