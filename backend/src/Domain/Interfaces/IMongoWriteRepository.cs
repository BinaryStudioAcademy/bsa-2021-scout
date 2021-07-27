using System.Threading.Tasks;
using MongoDB.Bson;
using Domain.Common;

namespace Domain.Interfaces
{
    public interface IMongoWriteRepository<T> where T : MongoEntity
    {
        Task<MongoEntity> CreateAsync(T entity);
        Task<MongoEntity> UpdateAsync(T entity);
        Task DeleteAsync(ObjectId id);
    }
}
