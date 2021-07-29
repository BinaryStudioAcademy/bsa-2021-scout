using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRTokenReadRepository
    {
        Task<RefreshToken> GetAsync(string token, string userId);
    }
}
