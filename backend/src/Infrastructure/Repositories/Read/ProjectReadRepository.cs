using Application.Common.Exceptions;
using Application.Interfaces;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class ProjectReadRepository : ReadRepository<Project>, IProjectReadRepository
    {
        protected readonly ICurrentUserContext _currentUserContext;

        public ProjectReadRepository(IConnectionFactory connectionFactory) : base("Projects", connectionFactory) { }


        public async Task<List<Project>> GetByCompanyIdAsync(string companyId)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            string sql = $"SELECT * FROM {_tableName} WHERE CompanyId = '{companyId}'";

            var projects = (await connection.QueryAsync<Project>(sql)).ToList();
            await connection.CloseAsync();
            return projects;
        }
    }
}
