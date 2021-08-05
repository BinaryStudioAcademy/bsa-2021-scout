using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Interfaces.Abstractions;
using Domain.Common;
using Application.Common.Exceptions;
using Infrastructure.Dapper.Interfaces;

namespace Infrastructure.Repositories.Abstractions
{
    public class ReadRepository<T> : IReadRepository<T>
        where T : Entity
    {
        protected readonly IConnectionFactory _connectionFactory;
        protected readonly string _tableName;

        public ReadRepository(string tableName, IConnectionFactory connectionFactory)
        {
            _tableName = tableName;
            _connectionFactory = connectionFactory;
        }

        public async Task<T> GetAsync(string id)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = $"SELECT * FROM {_tableName} WHERE Id = @id";
            T entity = await connection.QueryFirstOrDefaultAsync<T>(sql, new { id = id });

            if (entity == null)
            {
                throw new NotFoundException(typeof(T), id);
            }

            await connection.CloseAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> GetEnumerableAsync()
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            string sql = $"SELECT * FROM {_tableName}";

            var entities = await connection.QueryAsync<T>(sql);
            await connection.CloseAsync();

            return entities;
        }

        public async Task<T> GetByPropertyAsync(string property, string propertyValue)
        {
            using var connection = _connectionFactory.GetSqlConnection();
            string sql = $"SELECT * FROM {_tableName} WHERE [{property}] = @propertyValue";
            return await connection.QueryFirstOrDefaultAsync<T>(sql, new { propertyValue });
        }
    }
}
