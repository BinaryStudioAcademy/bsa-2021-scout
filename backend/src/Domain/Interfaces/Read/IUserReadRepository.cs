using Domain.Entities;
using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using System.Collections.Generic;

namespace Domain.Interfaces.Read
{
    public interface IUserReadRepository : IReadRepository<User>
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByCompanyIdAsync(string companyId);
        Task LoadRolesAsync(User user);
        Task<FileInfo> GetAvatarInfoAsync(string applicantId);
    }
}
