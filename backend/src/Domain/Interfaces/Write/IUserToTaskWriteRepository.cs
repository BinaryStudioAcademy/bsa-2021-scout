using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Write
{
    public interface IUserToTaskWriteRepository
    {
        Task UpdateUsersToTask(string [] newIds, string tasklId, string name, string description);
    }
}
