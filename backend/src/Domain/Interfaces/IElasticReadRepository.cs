using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces
{
    public interface IElasticReadRepository<T> : IReadRepository<T> where T : Entity
    {
        Task<IReadOnlyCollection<T>> SearchByQuery(string querty);
    }
}