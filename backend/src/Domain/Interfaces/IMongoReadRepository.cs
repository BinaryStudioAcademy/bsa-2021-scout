using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Domain.Common;

namespace Domain.Interfaces
{
    public interface IMongoReadRepository<T> where T : MongoEntity
    {
        Task<T> GetAsync(ObjectId id);
        Task<IEnumerable<T>> GetEnumerableAsync();
    }
}
