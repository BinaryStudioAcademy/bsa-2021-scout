using Domain.Entities;
using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using System.Collections.Generic;

namespace Domain.Interfaces.Read
{
    public interface ITaskReadRepository : IReadRepository<ToDoTask>
    {
        Task<ToDoTask> GetTaskWithTeamMembersByIdAsync(string id);
        Task<List<ToDoTask>> GetTasksWithTeamMembersAsync();
        Task<List<ToDoTask>> GetTasksWithTeamMembersByUserAsync(string userId);
    }
}
