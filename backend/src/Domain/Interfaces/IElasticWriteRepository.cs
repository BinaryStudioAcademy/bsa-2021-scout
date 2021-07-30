using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Common;
namespace Domain.Interfaces
{
    public interface IElasticWriteRepository<T> : IWriteRepository<T> where T : Entity
    {
        Task InsertBulk(IEnumerable<T> bulk);
    }
}