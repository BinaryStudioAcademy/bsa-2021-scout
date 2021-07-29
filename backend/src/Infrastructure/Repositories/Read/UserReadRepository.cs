using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Dapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class UserReadRepository : IUserReadRepository
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

        public async Task<User> GetByEmailAsync(string email)
        {
            using var connection = _connectionFactory.GetSqlConnection();
            string sql = "SELECT * FROM Users WHERE Email = @email";

            return await connection.QueryFirstAsync<User>(sql, new { email = email });
        }

        public Task<IEnumerable<User>> GetEnumerableAsync()
        {
            throw new NotImplementedException();
        }
    }
}
