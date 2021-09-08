using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Write;
using Infrastructure.EF;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Linq;
using System.Collections.Generic;

namespace Infrastructure.Repositories.Write
{
    public class UserToTaskWriteRepository : IUserToTaskWriteRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ApplicationDbContext _context;

        public UserToTaskWriteRepository(
            ApplicationDbContext context,
            IConnectionFactory connectionFactory
        )
        {
            _connectionFactory = connectionFactory;
            _context = context;
        }
        
        public async Task<ToDoTask> UpdateUsersToTask(ToDoTask updatedTask, List<string> newIds)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = $@"SELECT ToDoTaskId,UserId
                            FROM UserToTask
                            WHERE ToDoTaskId = @id";

            var teamMembersfromDB = await connection.QueryAsync<UserToTask>(sql, new { @id = updatedTask.Id });
                                  
            updatedTask.TeamMembers = (System.Collections.Generic.ICollection<UserToTask>)teamMembersfromDB;

            foreach (var user in updatedTask.TeamMembers.Where(at => newIds.Contains(at.UserId) == false).ToList())
            {
                _context.Remove(user);
            }

            foreach (var id in newIds.Except(updatedTask.TeamMembers.Select(x => x.UserId)))
            {
                _context.Add(new UserToTask() { UserId = id, ToDoTaskId = updatedTask.Id });
            }

            _context.Update(updatedTask);

            await _context.SaveChangesAsync();

            return updatedTask;
        }

        public async Task DeleteUsersToTask(string id)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = $@"SELECT ToDoTaskId,UserId
                            FROM UserToTask
                            WHERE ToDoTaskId = @id";

            var teamMembersfromDB = await connection.QueryAsync<UserToTask>(sql, new { @id = id });
                        

            foreach (var user in teamMembersfromDB)
            {
                _context.Remove(user);
            }                       

            await _context.SaveChangesAsync();
                        
        }
    }
}
