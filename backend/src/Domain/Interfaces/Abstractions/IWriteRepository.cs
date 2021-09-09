using Domain.Common;
using System.Threading.Tasks;

namespace Domain.Interfaces.Abstractions
{
    public interface IWriteRepository<T> where T : Entity
    {
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
