using Application.Common.Exceptions;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT *");
            sql.Append(" FROM Users");
            sql.Append(" LEFT JOIN UserToRoles ON UserToRoles.UserId = Users.Id");
            sql.Append(" LEFT JOIN Roles ON UserToRoles.RoleId = Roles.Id");
            sql.Append($" WHERE Users.Email = '{email}'");

            Dictionary<string, UserToRole> userToRoleDict = new Dictionary<string, UserToRole>();
            User cachedUser = null;

            IEnumerable<User> resultAsArray = await connection
                .QueryAsync<User, UserToRole, Role, User>(
                    sql.ToString(),
                    (user, userToRole, role) =>
                    {
                        if (cachedUser == null)
                        {
                            cachedUser = user;
                            cachedUser.UserRoles = new List<UserToRole>();
                        }

                        if (userToRole != null)
                        {
                            UserToRole userToRoleEntry;

                            if (!userToRoleDict.TryGetValue(userToRole.Id, out userToRoleEntry))
                            {
                                userToRoleEntry = userToRole;
                                userToRoleDict.Add(userToRoleEntry.Id, userToRoleEntry);
                                cachedUser.UserRoles.Add(userToRoleEntry);
                            }

                            if (role != null)
                            {
                                userToRoleEntry.Role = role;
                            }
                        }

                        return cachedUser;
                    },
                    splitOn: "Id,Id,Id"
                );

            User user = resultAsArray.Distinct().FirstOrDefault();

            await connection.CloseAsync();
            return user;
        }

        public async Task LoadRolesAsync(User user)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            var sql = new StringBuilder();
            sql.Append($"SELECT * FROM UserToRoles AS UR");
            sql.Append(" INNER JOIN Roles AS R ON UR.RoleId = R.Id");
            sql.Append(" WHERE UR.UserId = @userId;");

            var userRoles = await connection.QueryAsync<UserToRole, Role, UserToRole>(sql.ToString(),
            (ur, r) =>
            {
                ur.Role = r;
                return ur;
            },
            new { userId = user.Id });
            await connection.CloseAsync();
            user.UserRoles = userRoles as ICollection<UserToRole>;
        }
    }
}
