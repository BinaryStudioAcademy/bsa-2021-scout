using Domain.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Abstractions
{
    public interface IReadRepository<T> where T : Entity
    {
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetEnumerableAsync();
    }
}
