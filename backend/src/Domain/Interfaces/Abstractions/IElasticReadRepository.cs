using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Interfaces.Abstractions
{
    public interface IElasticReadRepository<T> : IReadRepository<T> where T : Entity
    {
        Task<IReadOnlyCollection<T>> SearchByQuery(string querty, CancellationToken _);
    }
}