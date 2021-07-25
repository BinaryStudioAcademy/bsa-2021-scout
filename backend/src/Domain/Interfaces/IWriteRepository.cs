using Domain.Common;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IWriteRepository<T> where T : Entity
    {
        Task<Entity> CreateAsync(T entity);
        Task<Entity> UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
