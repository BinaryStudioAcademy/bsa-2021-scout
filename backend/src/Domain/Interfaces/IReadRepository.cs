using Domain.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IReadRepository<T> where T: Entity
    {
        Task<T> GetAsync(Guid id);
        Task<IEnumerable<T>> GetEnumerableAsync();
    }
}
