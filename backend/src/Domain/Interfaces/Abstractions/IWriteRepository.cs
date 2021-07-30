using Domain.Common;
using System.Threading.Tasks;

namespace Domain.Interfaces.Abstractions
{
    public interface IWriteRepository<T> where T : Entity
    {
        Task<Entity> CreateAsync(T entity);
        Task<Entity> UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
