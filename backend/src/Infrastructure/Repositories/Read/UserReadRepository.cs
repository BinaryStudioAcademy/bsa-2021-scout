using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Dapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class UserReadRepository : IReadRepository<User>
    {
        private readonly IConnectionFactory _connectionFactory;

        public UserReadRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User> GetAsync(Guid id)
        {
            using var connection = _connectionFactory.GetSqlConnection();
            string sql = "SELECT * FROM Users WHERE Id = @id";

            return await connection.QueryFirstAsync<User>(sql, new { id = id });
        }

        public Task<IEnumerable<User>> GetEnumerableAsync()
        {
            throw new NotImplementedException();
        }
    }
}
