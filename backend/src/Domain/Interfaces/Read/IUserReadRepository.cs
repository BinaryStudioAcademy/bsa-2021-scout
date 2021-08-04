using Domain.Entities;
using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Read
{
    public interface IUserReadRepository : IReadRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task LoadRolesAsync(User user);
    }
}
