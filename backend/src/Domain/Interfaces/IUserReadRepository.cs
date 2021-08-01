using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserReadRepository: IReadRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task LoadRolesAsync(User user);
    }
}
