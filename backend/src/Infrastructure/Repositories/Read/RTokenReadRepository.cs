using Dapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class RTokenReadRepository : IRTokenReadRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public RTokenReadRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<RefreshToken> GetAsync(string token, string userId)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            string sql = "SELECT * FROM RefreshTokens WHERE UserId = @userId AND Token = @token";

            var tokenEntity = await connection.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { userId = userId, token = token });
            await connection.CloseAsync();
            return tokenEntity;
        }
    }
}
