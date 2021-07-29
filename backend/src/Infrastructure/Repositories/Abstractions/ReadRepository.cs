using System.Threading.Tasks;
using System.Collections.Generic;
using Dapper;
using Domain.Interfaces;
using Domain.Common;
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
            using var connection = _connectionFactory.GetSqlConnection();
            string sql = $"SELECT * FROM {_tableName} WHERE Id = @id";

            return await connection.QueryFirstAsync<T>(sql, new { id = id });
        }

        public async Task<IEnumerable<T>> GetEnumerableAsync()
        {
            using var connection = _connectionFactory.GetSqlConnection();
            string sql = $"SELECT * FROM {_tableName}";

            return await connection.QueryAsync<T>(sql);
        }
    }
}
