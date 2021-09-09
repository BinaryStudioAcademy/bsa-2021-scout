using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Write
{
    public interface IUserToTaskWriteRepository
    {
        Task<ToDoTask> UpdateUsersToTask(ToDoTask updatedTask, List<string> newIds);
        Task DeleteUsersToTask(string id);
    }
}
