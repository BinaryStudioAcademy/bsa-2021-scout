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

        public virtual async Task<T> GetAsync(string id)
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

        public virtual async Task<IEnumerable<T>> GetEnumerableAsync()
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            string sql = $"SELECT * FROM {_tableName}";

            var entities = await connection.QueryAsync<T>(sql);
            await connection.CloseAsync();

            return entities;
        }

        public virtual async Task<T> GetByPropertyAsync(string property, string propertyValue)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            string sql = $"SELECT * FROM {_tableName} WHERE [{property}] = @propertyValue";
            var entities = await connection.QueryFirstOrDefaultAsync<T>(sql, new { propertyValue });
            await connection.CloseAsync();
            return entities;
        }

        public virtual async Task<IEnumerable<T>> GetEnumerableByPropertyAsync(string property, string propertyValue)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            string sql = $"SELECT * FROM {_tableName} WHERE [{property}] = @propertyValue";
            var entities = await connection.QueryAsync<T>(sql, new { propertyValue });
            await connection.CloseAsync();
            return entities;
        }
    }
}
