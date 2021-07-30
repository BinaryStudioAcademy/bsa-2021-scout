using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        public UserReadRepository(IConnectionFactory connectionFactory) : base("Users", connectionFactory) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            string sql = $"SELECT * FROM {_tableName} WHERE Email = @email";

            var user = await connection.QueryFirstAsync<User>(sql, new { email = email });
            await connection.CloseAsync();
            return user;
        }
    }
}
