using Application.Common.Exceptions;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        public UserReadRepository(IConnectionFactory connectionFactory) : base("Users", connectionFactory) { }

        public async Task<User> GetByIdAsync(string id)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT *");
            sql.Append(" FROM Users");
            sql.Append(" LEFT JOIN UserToRoles ON UserToRoles.UserId = Users.Id");
            sql.Append(" LEFT JOIN Roles ON UserToRoles.RoleId = Roles.Id");
            sql.Append(" LEFT JOIN FileInfos ON FileInfos.Id = Users.AvatarId");
            sql.Append($" WHERE Users.Id = @id");

            Dictionary<string, UserToRole> userToRoleDict = new Dictionary<string, UserToRole>();
            User cachedUser = null;

            IEnumerable<User> resultAsArray = await connection
                .QueryAsync<User, UserToRole, Role, FileInfo, User>(
                    sql.ToString(),
                    (user, userToRole, role, fileinfo) =>
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
                        cachedUser.Avatar = fileinfo;
                        return cachedUser;
                    }, new { id = @id },
                    splitOn: "Id,Id,Id"
                );

            User user = resultAsArray.Distinct().FirstOrDefault();

            await connection.CloseAsync();
            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT *");
            sql.Append(" FROM Users");
            sql.Append(" LEFT JOIN UserToRoles ON UserToRoles.UserId = Users.Id");
            sql.Append(" LEFT JOIN Roles ON UserToRoles.RoleId = Roles.Id");
            sql.Append(" LEFT JOIN FileInfos ON FileInfos.Id = Users.AvatarId");
            sql.Append($" WHERE Users.Email = @email");

            Dictionary<string, UserToRole> userToRoleDict = new Dictionary<string, UserToRole>();
            User cachedUser = null;

            IEnumerable<User> resultAsArray = await connection
                .QueryAsync<User, UserToRole, Role, FileInfo, User>(
                    sql.ToString(),
                    (user, userToRole, role, fileinfo) =>
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
                        cachedUser.Avatar = fileinfo;
                        return cachedUser;
                    }, new { email = @email },
                    splitOn: "Id,Id,Id"
                );

            User user = resultAsArray.Distinct().FirstOrDefault();

            await connection.CloseAsync();
            return user;
        }
        public async Task<FileInfo> GetAvatarInfoAsync(string Id)
        {
            var connection = _connectionFactory.GetSqlConnection();

            var query = @"SELECT fi.* FROM Users a
                          INNER JOIN FileInfos fi ON a.AvatarId = fi.Id
                          WHERE a.Id = @Id";

            var fileInfo = await connection.QueryFirstOrDefaultAsync<FileInfo>(query, new { Id = @Id });

            if (fileInfo == null)
            {
                throw new Exception("The user "+Id+" wasn't found.");
            }

            await connection.CloseAsync();

            return fileInfo;
        }

        public async Task<IEnumerable<User>> GetUsersByCompanyIdAsync(string companyId)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT *");
            sql.Append(" FROM Users");
            sql.Append(" LEFT JOIN FileInfos ON FileInfos.Id = Users.AvatarId");
            sql.Append($" WHERE Users.CompanyId = @companyId");
            sql.Append($" ORDER BY Users.CreationDate DESC");


            User cachedUser = null;
            IEnumerable<User> users = await connection
                .QueryAsync<User,FileInfo, User>(
                    sql.ToString(),
                    (user, fileinfo) =>
                    {
                        cachedUser = user;
                        if(fileinfo != null)
                        {
                            cachedUser.Avatar = fileinfo;
                        }
                        return cachedUser;
                    }, new { companyId = @companyId },
                    splitOn: "Id,Id,Id"
                );


            //IEnumerable<User> users = await connection
            //    .QueryAsync<User>(sql.ToString(), new { companyId = @companyId });

            await connection.CloseAsync();
            return users;
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
