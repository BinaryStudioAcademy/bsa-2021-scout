using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Dapper.Interfaces;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class RTokenReadRepository: IRTokenReadRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public RTokenReadRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<RefreshToken> GetAsync(string token, Guid userId)
        {
            using var connection = _connectionFactory.GetSqlConnection();
            string sql = "SELECT * FROM RefreshTokens WHERE UserId = @userId AND token = @token";

            return await connection.QueryFirstAsync<RefreshToken>(sql, new { userId = userId, token = token });
        }
    }
}
